using StockTicker.Core.Common.Contracts;
using StockTicker.Core.Common.Models;

namespace StockTicker.Core.Tickers;

public class SearchSymbolCommand : IRequest<IEnumerable<StockSymbolEntry>>
{
    public required string SearchTerm { get; set; }
}

internal class SearchSymbolCommandHandler : IRequestHandler<SearchSymbolCommand, IEnumerable<StockSymbolEntry>>
{
    private readonly ILogger<SearchSymbolCommandHandler> _logger;
    private readonly IStockProvider _stockProvider;

    public SearchSymbolCommandHandler(IStockProvider stockProvider, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SearchSymbolCommandHandler>();
        _stockProvider = stockProvider;
    }

    public Task<IEnumerable<StockSymbolEntry>> Handle(SearchSymbolCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handle {Command} term=[{SearchTerm}]", nameof(SearchSymbolCommand), request.SearchTerm);
        return _stockProvider.SearchSymbolAsync(request.SearchTerm, cancellationToken);
    }
}
