using System.Collections.Concurrent;
using System.Linq.Expressions;
using Azure;
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

    public async Task<bool> Exists(string table, Expression<Func<T, bool>> where, CancellationToken cancellationToken)
    {
        TableClient tableClient = await GetTableClientAsync(table, cancellationToken).ConfigureAwait(false);
        AsyncPageable<T> queryResults = tableClient.QueryAsync(where, 4, ["PartitionKey"], cancellationToken);
        T? e = await queryResults.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        return e is not null;
    }

    public async Task<T> GetEntityAsync(string table, string partition, string id, CancellationToken cancellationToken)
    {
        TableClient tableClient = await GetTableClientAsync(table, cancellationToken).ConfigureAwait(false);
        return await tableClient.GetEntityAsync<T>(partition, id, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<T>> QueryEntitiesAsync(string table, Expression<Func<T, bool>> where, CancellationToken cancellationToken)
    {
        TableClient tableClient = await GetTableClientAsync(table, cancellationToken).ConfigureAwait(false);
        AsyncPageable<T> queryResults = tableClient.QueryAsync(where, cancellationToken: cancellationToken);

        ConcurrentBag<T> entities = [];
        await foreach (Page<T> page in queryResults.AsPages().ConfigureAwait(false))
        {
            foreach (T item in page.Values)
            {
                entities.Add(item);
            }
        }

        return entities;
    }

    public async Task UpsertEntitiesAsync(string table, IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        TableClient tableClient = await GetTableClientAsync(table, cancellationToken).ConfigureAwait(false);

        List<List<T>> chuncks = entities
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 100)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

        foreach (List<T> chunck in chuncks)
        {
            IEnumerable<TableTransactionAction> actions = chunck.Select(e => new TableTransactionAction(TableTransactionActionType.UpsertMerge, e));
            Response<IReadOnlyList<Response>> results = await tableClient.SubmitTransactionAsync(actions, cancellationToken).ConfigureAwait(false);


        }
    }

    public async Task<T> UpsertEntityAsync(string table, T entity, CancellationToken cancellationToken)
    {
        TableClient tableClient = await GetTableClientAsync(table, cancellationToken).ConfigureAwait(false);
        await tableClient.UpsertEntityAsync(entity, cancellationToken: cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public async Task DeleteEntityAsync(string table, string category, string id, CancellationToken cancellationToken)
    {
        var tableClient = await GetTableClientAsync(table, cancellationToken).ConfigureAwait(false);
        await tableClient.DeleteEntityAsync(category, id, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    private async Task<TableClient> GetTableClientAsync(string tableName, CancellationToken cancellationToken)
    {
        TableServiceClient serviceClient = new(_settings.Connection);
        TableClient tableClient = serviceClient.GetTableClient(tableName);
        await tableClient.CreateIfNotExistsAsync(cancellationToken).ConfigureAwait(false);
        return tableClient;
    }
}
