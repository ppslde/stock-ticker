using System.Security.Cryptography;
using StockTicker.Core.Common.Contracts;
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
            .MapGroup(this, "weather")
            //.RequireAuthorization()
            .MapGet(Get)
            .MapGet(Get2, "info");
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

    private IEnumerable<string> Get2(ICurrentUser currentUser, HttpContext httpContext)
    {
        List<string> data = [
            currentUser.UserName,
            currentUser.UserId.ToString(),
            ];

        data.Add("REQ_HEADERS:");
        data.AddRange(httpContext.Request.Headers.Select(h => $"{h.Key}:{h.Value}"));
        data.Add("SESSION_KEYS:");
        data.AddRange(httpContext.Session?.Keys ?? []);
        data.Add("CLAIMS:");
        data.AddRange(httpContext.User.Claims.Select(c => $"{c.ValueType}:{c.Value}"));

        return data;
    }
}
