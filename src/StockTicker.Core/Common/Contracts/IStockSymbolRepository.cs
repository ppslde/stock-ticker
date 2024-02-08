using StockTicker.Core.Common.Models;

namespace StockTicker.Core.Common.Contracts;

public interface IStockSymbolRepository
{
    Task<IEnumerable<StockSymbolEntry>> GetEnabledTickers(CancellationToken cancellationToken);
    Task SaveStockEntry(StockSymbolEntry stockSymbol, CancellationToken cancellationToken);
}
