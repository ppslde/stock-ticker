using Azure;
using Azure.Data.Tables;

namespace StockTicker.Infrastructure.Storage.Models;

internal class SymbolTableEntry : ITableEntity
{
    public required string PartitionKey { get; set; } // =>     public required string Type { get; set; }
    public required string RowKey { get; set; } // => public required string Symbol { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public required string Name { get; set; }
    public required string Region { get; set; }
    public required string TimeZone { get; set; }
    public required string Currency { get; set; }
    public string? MarketOpen { get; set; }
    public string? MarketClose { get; set; }
    public bool UseInDaílyUpdate { get; set; }
}
