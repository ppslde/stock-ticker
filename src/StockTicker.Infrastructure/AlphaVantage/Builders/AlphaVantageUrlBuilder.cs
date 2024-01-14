using Flurl;

namespace StockTicker.Infrastructure.AlphaVantage.Builders;

internal class AlphaVantageUrl
{
    private readonly string? _baseUrl;
    private string? _apiKey;

    private FunctionQueryBuilder? _currentFunctionQuery;

    public AlphaVantageUrl(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public AlphaVantageUrl WithApiKey(string apiKey)
    {
        _apiKey = apiKey;
        return this;
    }

    public SearchFunctionBuilder Search()
    {
        SearchFunctionBuilder builder = new(this);
        _currentFunctionQuery = builder;
        return builder;
    }

    public DailySeriesFunctionBuilder DailySeries()
    {
        DailySeriesFunctionBuilder builder = new(this);
        _currentFunctionQuery = builder;
        return builder;
    }

    public string Build()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_baseUrl, "BaseUrl");
        ArgumentNullException.ThrowIfNull(_currentFunctionQuery, "QueryFunction");
        ArgumentException.ThrowIfNullOrWhiteSpace(_apiKey, "ApiKey");

        return new Url(_baseUrl)
                     .AppendPathSegment("query")
                     .SetQueryParams(_currentFunctionQuery.BuildInternal())
                     .SetQueryParam("apikey", _apiKey)
                     .ToString();
    }
}