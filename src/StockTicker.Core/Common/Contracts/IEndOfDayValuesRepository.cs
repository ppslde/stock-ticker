using StockTicker.Core.Common.Models;

namespace StockTicker.Core.Common.Contracts;

public interface IEndOfDayValuesRepository
{
    Task InsertEndOfDayValuesAsync(StockSymbolEntry stockSymbol, IEnumerable<EndOfDayEntry> items, CancellationToken cancellationToken);
}
