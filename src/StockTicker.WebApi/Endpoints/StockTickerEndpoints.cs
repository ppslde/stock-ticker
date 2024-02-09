using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockTicker.Core.Common.Models;
using StockTicker.Core.Tickers;
using StockTicker.WebApi.Common;

namespace StockTicker.WebApi.Controllers;

public class StockTickerEndpoints : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGroup(this, "stock-ticker")
            //.RequireAuthorization()
            .MapGet(SearchSymbol, "search")
            .MapPost(SaveSymbol);
    }

    async Task<IEnumerable<StockSymbolEntry>> SearchSymbol(ISender sender, [FromQuery] string SearchTerm, CancellationToken cancellationToken)
    {
        IEnumerable<StockSymbolEntry> r = await sender.Send(new SearchSymbolCommand() { SearchTerm = SearchTerm }, cancellationToken).ConfigureAwait(false);
        return r;
    }

    async Task SaveSymbol(ISender sender, StockSymbolEntry symbol, CancellationToken cancellationToken)
    {
        await sender.Send(new SaveSymbolCommand() { Symbol = symbol }, cancellationToken).ConfigureAwait(false);
    }
}
