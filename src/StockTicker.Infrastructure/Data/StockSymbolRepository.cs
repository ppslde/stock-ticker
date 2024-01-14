using Microsoft.Extensions.Logging;
using StockTicker.Core.Common.Contracts;
using StockTicker.Core.Common.Models;
using StockTicker.Infrastructure.Storage.Common;
using StockTicker.Infrastructure.Storage.Models;

namespace StockTicker.Infrastructure.Data;

internal class StockSymbolRepository : IStockSymbolRepository
{
    private readonly ILogger<StockSymbolRepository> _logger;
    private readonly IStorage<SymbolTableEntry> _storage;

    public StockSymbolRepository(IStorage<SymbolTableEntry> storage, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<StockSymbolRepository>();
        _storage = storage;
    }

    public async Task SaveStockEntry(StockSymbolEntry stockSymbol, CancellationToken cancellationToken)
    {
        SymbolTableEntry entry = new()
        {
            PartitionKey = stockSymbol.Type,
            RowKey = stockSymbol.Key,
            Name = stockSymbol.Name,
            Region = stockSymbol.Region,
            TimeZone = stockSymbol.TimeZone,
            Currency = stockSymbol.Currency,
            MarketClose = stockSymbol.MarketClose.ToString(),
            MarketOpen = stockSymbol.MarketOpen.ToString(),
            UseInDaílyUpdate = stockSymbol.UseInDaílyUpdate
        };

        await _storage.UpsertEntityAsync("StockSymbols", entry, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Stock symbol saved: partition=[{PartitionKey}] row=[{RowKey}]", entry.PartitionKey, entry.RowKey);
    }
}
