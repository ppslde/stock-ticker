using StockTicker.Core.Common.Models;

namespace StockTicker.Core.Common.Contracts;
public interface IStockProvider
{
    Task<IEnumerable<StockSymbolEntry>> SearchSymbolAsync(string searchTerm);
}
