using StockTicker.Core.Common.Models;

namespace StockTicker.Core.Common.Contracts;
public interface IStockProvider
{
    Task<IEnumerable<EndOfDayEntry>> GetEndOfDayDataAsync(StockSymbolEntry stockSymbol, CancellationToken cancellationToken);
    Task<IEnumerable<StockSymbolEntry>> SearchSymbolAsync(string searchTerm, CancellationToken cancellationToken);
}
