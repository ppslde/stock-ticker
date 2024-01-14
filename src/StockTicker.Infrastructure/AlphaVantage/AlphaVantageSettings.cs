namespace StockTicker.Infrastructure.AlphaVantage;

public sealed class AlphaVantageSettings
{
    public required string BaseUrl { get; set; }
    public List<string> ApiKeys { get; set; } = [];
}
