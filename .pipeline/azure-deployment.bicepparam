using './azure-deployment.bicep'

param containerUsername = readEnvironmentVariable('CONTAINERREGISTRY_USERNAME', '')
param containerPassword = readEnvironmentVariable('CONTAINERREGISTRY_PASSWORD', '')
param containerRegistry = readEnvironmentVariable('CONTAINERREGISTRY', '')

param authClientId = readEnvironmentVariable('AUTH_CLIENTID', '')
param authClientSecret = readEnvironmentVariable('AUTH_CLIENTSECRET', '')

param eodRunnerImage =  readEnvironmentVariable('DEPLOY_IMAGE_EODRUNNER','')
param webApiImage = readEnvironmentVariable('DEPLOY_IMAGE_WEBAPI','')
param containerTag =readEnvironmentVariable('DEPLOY_CONTAINER_TAG','')

param alphaVantageKey0 = readEnvironmentVariable('ALPHA_VANTAGE_KEY_0', '')
param alphaVantageKey1 = readEnvironmentVariable('ALPHA_VANTAGE_KEY_1', '')
param alphaVantageKey2 = readEnvironmentVariable('ALPHA_VANTAGE_KEY_2', '')
