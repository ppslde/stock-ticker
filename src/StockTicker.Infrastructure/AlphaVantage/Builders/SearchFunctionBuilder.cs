namespace StockTicker.Infrastructure.AlphaVantage.Builders;

internal sealed class SearchFunctionBuilder : FunctionQueryBuilder
{
    private string? _searchTerm;

    internal SearchFunctionBuilder(AlphaVantageUrl parent) : base(parent, "SYMBOL_SEARCH")
    { }

    public SearchFunctionBuilder ForTerm(string searchterm)
    {
        _searchTerm = searchterm;
        return this;
    }

    internal override Dictionary<string, string> BuildInternal()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_searchTerm, "Keywords");

        return new Dictionary<string, string>
        {
            ["function"] = Function,
            ["keywords"] = _searchTerm,
        };
    }
}
