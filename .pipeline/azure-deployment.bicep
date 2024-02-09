targetScope = 'resourceGroup'

@description('The location to which resources are deployed. Default: the resource group scope')
param location string = resourceGroup().location

@description('Container registry')
param containerRegistry string

@description('User name for container registry')
param containerUsername string

@description('Password for container registry user name')
@secure()
param containerPassword string

@description('ClientId for app registration used for web api auth')
param authClientId string

@description('ClientSecret for app registration used for web api auth')
@secure()
param authClientSecret string

@description('Docker image to use for the EodRunner')
param eodRunnerImage string

@description('Docker image to use for the WebApi')
param webApiImage string

@description('Container tag for deployment')
param containerTag string

param alphaVantageKey0 string
param alphaVantageKey1 string
param alphaVantageKey2 string

var eodRunnerImmageFull = '${containerRegistry}/${eodRunnerImage}:${containerTag}'
var webApiImageFull = '${containerRegistry}/${webApiImage}:${containerTag}'

resource dataStorage 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: 'ppslstockdata'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

var dataStorageConnection = 'DefaultEndpointsProtocol=https;AccountName=${dataStorage.name};AccountKey=${dataStorage.listKeys().keys[0].value};EndpointSuffix=${environment().suffixes.storage}'

resource dataStorageBlobs 'Microsoft.Storage/storageAccounts/blobServices@2023-01-01' = {
  parent: dataStorage
  name: 'default'
  properties: {
    containerDeleteRetentionPolicy: {
      enabled: true
      days: 3
    }
    deleteRetentionPolicy: {
      allowPermanentDelete: false
      enabled: true
      days: 3
    }
    isVersioningEnabled: false
  }
}

resource dataStorageContainerDeadletter 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = {
  parent: dataStorageBlobs
  name: 'stockarchiv'
}

resource logWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: 'ppsl-stock-log-workspace'
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: 'ppsl-stock-app-insights'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Redfield'
    Request_Source: 'IbizaAIExtension'
    RetentionInDays: 30
    WorkspaceResourceId: logWorkspace.id
    IngestionMode: 'LogAnalytics'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

resource containerAppsEnvironment 'Microsoft.App/managedEnvironments@2023-05-01' = {
  name: 'ppsl-stock-containerEnvironment'
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logWorkspace.properties.customerId
        sharedKey: logWorkspace.listKeys().primarySharedKey
      }
    }
  }
}

resource eodRunnerApp 'Microsoft.App/jobs@2023-05-01' = {
  name: 'ppsl-stock-eod-runner'
  location: location
  properties: {
    environmentId: containerAppsEnvironment.id
    configuration: {
      triggerType: 'Schedule'
      replicaTimeout: 55
      replicaRetryLimit: 0
      scheduleTriggerConfig: {
        replicaCompletionCount: 1
        cronExpression: '*/1 * * * *'
        parallelism: 1
      }
      secrets: [
        {
          name: 'registry-secret'
          value: containerPassword
        }
        {
          name: 'appinsights-secret'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'data-storage-secret'
          value: dataStorageConnection
        }
      ]
      registries: [
        {
          server: containerRegistry
          username: containerUsername
          passwordSecretRef: 'registry-secret'
        }
      ]
    }

    template: {
      containers: [
        {
          image: eodRunnerImmageFull
          name: 'stock-eod-runner'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'ApplicationInsights__ConnectionString'
              secretRef: 'appinsights-secret'
            }
            {
              name: 'ConnectionStrings__DataStorageAccount'
              secretRef: 'data-storage-secret'
            }
          ]
        }
      ]
    }
  }
}


resource webApiApp 'Microsoft.App/containerApps@2023-05-01' = {
  name: 'ppsl-stock-web-api'
  location: location
  properties: {
    managedEnvironmentId: containerAppsEnvironment.id
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 8080
        transport: 'http'
        allowInsecure: false
        traffic: [
          {
            weight: 100
            latestRevision: true
          }
        ]
        corsPolicy: {
          allowCredentials: true
          allowedOrigins: [
            '*'
          ]
        }
      }
      secrets: [
        {
          name: 'registry-secret'
          value: containerPassword
        }
        {
          name: 'microsoft-provider-authentication-secret'
          value: authClientSecret
        }
        {
          name: 'appinsights-secret'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'data-storage-secret'
          value: dataStorageConnection
        }
        {
          name: 'alpha-vantage-key-0'
          value: alphaVantageKey0
        }
        {
          name: 'alpha-vantage-key-1'
          value: alphaVantageKey1
        }
        {
          name: 'alpha-vantage-key-2'
          value: alphaVantageKey2
        }
      ]
      registries: [
        {
          server: containerRegistry
          username: containerUsername
          passwordSecretRef: 'registry-secret'
        }
      ]
    }
    template: {
      containers: [
        {
          image: webApiImageFull
          name: 'stock-web-api'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          probes: [
            {
              failureThreshold: 3
              initialDelaySeconds: 10
              timeoutSeconds: 10
              httpGet: {
                path: '/healthy'
                port: 8080
                scheme: 'HTTP'
              }
            }
          ]
          env: [
            {
              name: 'ApplicationInsights__ConnectionString'
              secretRef: 'appinsights-secret'
            }
            {
              name: 'AzTableStorageSettings__Connection'
              secretRef: 'data-storage-secret'
            }
            {
              name: 'AlphaVantageSettings__ApiKeys__0'
              secretRef: 'alpha-vantage-key-0'
            }
            {
              name: 'AlphaVantageSettings__ApiKeys__1'
              secretRef: 'alpha-vantage-key-1'
            }
            {
              name: 'AlphaVantageSettings__ApiKeys__2'
              secretRef: 'alpha-vantage-key-2'
            }
          ]
        }
      ]

      scale: {
        minReplicas: 0
        maxReplicas: 2
        rules: [
          {
            name: 'http-scale'
            http: {
              metadata: {
                concurrentRequests: '40'
              }
            }
          }
        ]
      }
    }
  }
}

resource webApiAppAuth 'Microsoft.App/containerApps/authConfigs@2023-05-01' = {
  parent: webApiApp
  name: 'current'
  properties: {
    platform: {
      enabled: true
    }
    globalValidation: {
      unauthenticatedClientAction: 'RedirectToLoginPage'
      redirectToProvider: 'azureactivedirectory'
    }
    identityProviders: {
      azureActiveDirectory: {
        registration: {
          openIdIssuer: 'https://sts.windows.net/${subscription().tenantId}/v2.0'
          clientId: authClientId
          clientSecretSettingName: 'microsoft-provider-authentication-secret'
        }
        validation: {
          allowedAudiences: []
        }
        isAutoProvisioned: false
      }
    }
    login: {
      routes: {}
      preserveUrlFragmentsForLogins: false
      cookieExpiration: {}
      nonce: {}
    }
  }
}
