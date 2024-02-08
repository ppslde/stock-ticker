using System.Linq.Expressions;

namespace StockTicker.Infrastructure.Storage.Common;

public interface IStorage<T>
{
    Task<T> GetEntityAsync(string table, string category, string id, CancellationToken cancellationToken);
    Task<T> UpsertEntityAsync(string table, T entity, CancellationToken cancellationToken);
    Task DeleteEntityAsync(string table, string category, string id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> QueryEntitiesAsync(string table, Expression<Func<T, bool>> where, CancellationToken cancellationToken);
    Task UpsertEntitiesAsync(string table, IEnumerable<T> entities, CancellationToken cancellationToken);
    Task<bool> Exists(string table, Expression<Func<T, bool>> where, CancellationToken cancellationToken);
}
