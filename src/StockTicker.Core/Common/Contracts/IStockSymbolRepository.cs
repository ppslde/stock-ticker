using StockTicker.Core.Common.Models;

namespace StockTicker.Core.Common.Contracts;

public interface IStockSymbolRepository
{
    Task SaveStockEntry(StockSymbolEntry stockSymbol, CancellationToken cancellationToken);
}
