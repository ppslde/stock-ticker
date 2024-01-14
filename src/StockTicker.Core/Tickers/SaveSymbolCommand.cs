using StockTicker.Core.Common.Contracts;
using StockTicker.Core.Common.Models;

namespace StockTicker.Core.Tickers;

public class SaveSymbolCommand : IRequest
{
    public required StockSymbolEntry Symbol { get; set; }
}

internal class SaveSymbolCommandHandler : IRequestHandler<SaveSymbolCommand>
{
    private readonly ILogger<SaveSymbolCommandHandler> _logger;
    private readonly IStockSymbolRepository _symbols;

    public SaveSymbolCommandHandler(IStockSymbolRepository symbols, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SaveSymbolCommandHandler>();
        _symbols = symbols;
    }

    public Task Handle(SaveSymbolCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handle {Command} symbol=[{Key}]", nameof(SaveSymbolCommand), request.Symbol.Key);
        return _symbols.SaveStockEntry(request.Symbol, cancellationToken);
    }
}
