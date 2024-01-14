using System.Text.Json.Serialization;

namespace StockTicker.Infrastructure.AlphaVantage.Models;
internal class SearchData
{
    [JsonPropertyName("1. symbol")]
    public required string Symbol { get; set; }

    [JsonPropertyName("2. name")]
    public required string Name { get; set; }

    [JsonPropertyName("3. type")]
    public required string Type { get; set; }

    [JsonPropertyName("4. region")]
    public required string Region { get; set; }

    [JsonPropertyName("5. marketOpen")]
    public required string MarketOpen { get; set; }

    [JsonPropertyName("6. marketClose")]
    public required string MarketClose { get; set; }

    [JsonPropertyName("7. timezone")]
    public required string TimeZone { get; set; }

    [JsonPropertyName("8. currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("9. matchScore"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal MatchScore { get; set; }
}

/*
 "1. symbol": "BA",
    "2. name": "Boeing Company",
    "3. type": "Equity",
    "4. region": "United States",
    "5. marketOpen": "09:30",
    "6. marketClose": "16:00",
    "7. timezone": "UTC-04",
    "8. currency": "USD",
    "9. matchScore": "1.0000"
 
 */
