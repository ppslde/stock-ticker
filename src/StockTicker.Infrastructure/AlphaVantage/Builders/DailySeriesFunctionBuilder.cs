namespace StockTicker.Infrastructure.AlphaVantage.Builders;

internal sealed class DailySeriesFunctionBuilder : FunctionQueryBuilder
{
    private string? _symbol;
    private OutputSize _outputSize = OutputSize.FULL;

    public DailySeriesFunctionBuilder(AlphaVantageUrl url) : base(url, "TIME_SERIES_DAILY")
    {
    }

    public DailySeriesFunctionBuilder ForSymbol(string symbol)
    {
        _symbol = symbol;
        return this;
    }

    public DailySeriesFunctionBuilder WithSize(OutputSize outputSize)
    {
        _outputSize = outputSize;
        return this;
    }

    internal override Dictionary<string, string> BuildInternal()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_symbol, "Symbol");

        return new Dictionary<string, string>
        {
            ["function"] = Function,
            ["symbol"] = _symbol,
            ["outputsize"] = _outputSize.ToString().ToLower()
        };
    }

    public enum OutputSize
    {
        FULL,
        COMPACT
    }
}
