using System.Runtime.CompilerServices;
using StockTicker.Core.Common.Contracts;
using StockTicker.Core.Common.Models;

namespace StockTicker.Core.DailyData;

public class UpdateEndOfDayEnabledTickerCommand : IRequest
{
}

internal class UpdateEndOfDayEnabledTickerCommandHandler : IRequestHandler<UpdateEndOfDayEnabledTickerCommand>
{
    private readonly ILogger<UpdateEndOfDayEnabledTickerCommandHandler> _logger;
    private readonly IStockSymbolRepository _symbols;
    private readonly IStockProvider _stockProvider;
    private readonly IEndOfDayValuesRepository _endOfDayValues;

    public UpdateEndOfDayEnabledTickerCommandHandler(IStockSymbolRepository symbols, IStockProvider stockProvider, IEndOfDayValuesRepository endOfDayValues, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<UpdateEndOfDayEnabledTickerCommandHandler>();
        _symbols = symbols;
        _stockProvider = stockProvider;
        _endOfDayValues = endOfDayValues;
    }

    public async Task Handle(UpdateEndOfDayEnabledTickerCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<StockSymbolEntry> enabledSymbols = await _symbols.GetEnabledTickers(cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("{Count} enabled tickers for end of day update found.", enabledSymbols.Count());

        if (!enabledSymbols.Any())
            return;

        await foreach ((StockSymbolEntry symbol, IEnumerable<EndOfDayEntry> eods) in GetEodValues(enabledSymbols, cancellationToken).WithCancellation(cancellationToken))
        {
            _logger.LogDebug("{Count} end of day values found for symbol {Key}", eods.Count(), symbol.Key);
            //await _endOfDayValues.StoreSth()

        }
    }

    private async IAsyncEnumerable<(StockSymbolEntry, IEnumerable<EndOfDayEntry>)> GetEodValues(IEnumerable<StockSymbolEntry> symbols, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (StockSymbolEntry symbol in symbols)
            yield return (symbol, await _stockProvider.GetEndOfDayDataAsync(symbol, cancellationToken));
    }
}
