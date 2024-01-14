namespace StockTicker.Core.Common.Models;

public class StockSymbolEntry
{
    public required string Key { get; set; }
    public required string Name { get; set; }
    public required string Region { get; set; }
    public TimeOnly MarketOpen { get; set; }
    public TimeOnly MarketClose { get; set; }
    public required string Type { get; set; }
    public required string Currency { get; set; }
    public required string TimeZone { get; set; }
    public bool UseInDaílyUpdate { get; set; }
}
