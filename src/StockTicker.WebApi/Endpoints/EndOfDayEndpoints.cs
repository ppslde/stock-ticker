using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockTicker.Core.Common.Models;
using StockTicker.Core.Tickers;
using StockTicker.WebApi.Common;

namespace StockTicker.WebApi.Endpoints;

public class EndOfDayEndpoints : EndpointGroupBase
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {

        endpoints
            .MapGroup(this)
            //.RequireAuthorization()
            .MapPost(UpdateSymbolEod, "update");

    }

    async Task UpdateSymbolEod(ISender sender, [FromBody] StockSymbolEntry symbol, CancellationToken cancellationToken)
    {
        await sender.Send(new SaveSymbolCommand() { Symbol = symbol }, cancellationToken).ConfigureAwait(false);
    }
}
