namespace StockTicker.Infrastructure.AlphaVantage.Builders;
internal abstract class FunctionQueryBuilder
{
    protected AlphaVantageUrl AlphaVantageUrl { get; }
    protected string Function { get; }

    protected FunctionQueryBuilder(AlphaVantageUrl url, string functionId)
    {
        ArgumentNullException.ThrowIfNull(url, "AlphaVantageUrl");
        ArgumentException.ThrowIfNullOrWhiteSpace(functionId, "Function");

        AlphaVantageUrl = url;
        Function = functionId;
    }

    //public string Build() => AlphaVantageUrl.Build();

    public AlphaVantageUrl WithApiKey(string apiKey) => AlphaVantageUrl.WithApiKey(apiKey);

    internal abstract Dictionary<string, string> BuildInternal();
}
