using Microsoft.Extensions.Logging;
using StockTicker.Core.Common.Contracts;
using StockTicker.Core.Common.Models;
using StockTicker.Infrastructure.Storage.Common;
using StockTicker.Infrastructure.Storage.Models;

namespace StockTicker.Infrastructure.Data;

internal class EndOfDayValuesRepository : IEndOfDayValuesRepository
{
    private const string _tableName = "EndOfDayStocks";

    private readonly IStorage<EodTableEntry> _storage;
    private readonly ILogger _logger;

    public EndOfDayValuesRepository(IStorage<EodTableEntry> storage, ILoggerFactory loggerFactory)
    {
        _storage = storage;
        _logger = loggerFactory.CreateLogger<EndOfDayValuesRepository>();
    }

    public async Task InsertEndOfDayValuesAsync(StockSymbolEntry stockSymbol, IEnumerable<EndOfDayEntry> items, CancellationToken cancellationToken)
    {
        IEnumerable<Task> upsertTasksx = items
            .Where(i => i.Symbol == stockSymbol.Key)
            .Select(i => new EodTableEntry { PartitionKey = i.Symbol, RowKey = i.Date.ToString(), Open = i.Open, High = i.High, Low = i.Low, Close = i.Close, Volume = i.Volume })
            .GroupBy(i => i.PartitionKey, i => i)
            .Select(g => _storage.UpsertEntitiesAsync(_tableName, g.ToList(), cancellationToken));

        await Task.WhenAll(upsertTasksx).ConfigureAwait(false);
    }
}
