using System.Text.Json.Serialization;

namespace StockTicker.Infrastructure.AlphaVantage.Models;
internal class EodData
{
    [JsonPropertyName("1. open"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required decimal Open { get; set; }

    [JsonPropertyName("2. high"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required decimal High { get; set; }

    [JsonPropertyName("3. low"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required decimal Low { get; set; }

    [JsonPropertyName("4. close"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required decimal Close { get; set; }

    [JsonPropertyName("5. volume"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required int Volume { get; set; }
}
