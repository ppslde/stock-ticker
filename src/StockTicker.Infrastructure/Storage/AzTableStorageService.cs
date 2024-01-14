using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using StockTicker.Infrastructure.Storage.Common;

namespace StockTicker.Infrastructure.Storage;

internal class AzTableStorageService<T> : IStorage<T> where T : class, ITableEntity
{
    private readonly AzTableStorageSettings _settings;

    public AzTableStorageService(IOptions<AzTableStorageSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<T> GetEntityAsync(string table, string partition, string id, CancellationToken cancellationToken)
    {
        var tableClient = await GetTableClientAsync(table, cancellationToken).ConfigureAwait(false);
        return await tableClient.GetEntityAsync<T>(partition, id, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<T> UpsertEntityAsync(string table, T entity, CancellationToken cancellationToken)
    {
        var tableClient = await GetTableClientAsync(table, cancellationToken).ConfigureAwait(false);
        await tableClient.UpsertEntityAsync(entity, cancellationToken: cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public async Task DeleteEntityAsync(string table, string category, string id, CancellationToken cancellationToken)
    {
        var tableClient = await GetTableClientAsync(table, cancellationToken).ConfigureAwait(false);
        await tableClient.DeleteEntityAsync(category, id).ConfigureAwait(false);
    }

    private async Task<TableClient> GetTableClientAsync(string tableName, CancellationToken cancellationToken)
    {
        TableServiceClient serviceClient = new(_settings.Connection);
        TableClient tableClient = serviceClient.GetTableClient(tableName);
        await tableClient.CreateIfNotExistsAsync(cancellationToken).ConfigureAwait(false);
        return tableClient;
    }
}
