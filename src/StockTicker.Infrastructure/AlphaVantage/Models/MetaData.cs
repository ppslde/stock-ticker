using System.Text.Json.Serialization;

namespace StockTicker.Infrastructure.AlphaVantage.Models;

internal class MetaData
{
    [JsonPropertyName("1. Information")]
    public required string Information { get; set; }

    [JsonPropertyName("2. Symbol")]
    public required string Symbol { get; set; }

    [JsonPropertyName("3. Last Refreshed")]
    public required string LastRefreshed { get; set; }

    [JsonPropertyName("4. Output Size")]
    public required string OutputSize { get; set; }

    [JsonPropertyName("5. Time Zone")]
    public required string TimeZone { get; set; }
}