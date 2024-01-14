namespace StockTicker.Infrastructure.Storage.Common;

public interface IStorage<T>
{
    Task<T> GetEntityAsync(string table, string category, string id, CancellationToken cancellationToken);
    Task<T> UpsertEntityAsync(string table, T entity, CancellationToken cancellationToken);
    Task DeleteEntityAsync(string table, string category, string id, CancellationToken cancellationToken);
}
