using System.Security.Cryptography;
using StockTicker.WebApi.Common;

namespace StockTicker.WebApi.Controllers;

public class WeatherForecastEndpoints : EndpointGroupBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


    public override void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGroup(this)
            //.RequireAuthorization()
            .MapGet(Get);
    }

    private IEnumerable<WeatherForecast> Get(ILogger<WeatherForecastEndpoints> logger)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = RandomNumberGenerator.GetInt32(-20, 55),
            Summary = Summaries[RandomNumberGenerator.GetInt32(Summaries.Length)]
        })
        .ToArray();
    }
}
