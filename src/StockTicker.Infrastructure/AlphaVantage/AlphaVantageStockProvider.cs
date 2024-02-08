using System.Text.Json;
using Microsoft.Extensions.Options;
using StockTicker.Core.Common.Contracts;
using StockTicker.Core.Common.Models;
using StockTicker.Infrastructure.AlphaVantage.Builders;
using StockTicker.Infrastructure.AlphaVantage.Common;
using StockTicker.Infrastructure.AlphaVantage.Models;

namespace StockTicker.Infrastructure.AlphaVantage;
internal class AlphaVantageStockProvider : IStockProvider
{
    private readonly AlphaVantageSettings _settings;
    private readonly IApiKeyProvider _apiKeyProvider;

    public AlphaVantageStockProvider(IOptions<AlphaVantageSettings> options, IApiKeyProvider apiKeyProvider)
    {
        _settings = options.Value;
        _apiKeyProvider = apiKeyProvider;
    }

    public async Task<IEnumerable<EndOfDayEntry>> GetEndOfDayDataAsync(StockSymbolEntry stockSymbol, CancellationToken cancellationToken)
    {
        DailySeriesFunctionBuilder url = new AlphaVantageUrl(_settings.BaseUrl)
                                                .DailySeries()
                                                .ForSymbol(stockSymbol.Key);

        Dictionary<string, JsonElement> result = await QueryDataAsync(url, cancellationToken).ConfigureAwait(false);

        MetaData? metaData = result?.ElementAt(0).Value.Deserialize<MetaData>();
        Dictionary<DateOnly, JsonElement> eodResults = result?.ElementAt(1).Value.Deserialize<Dictionary<DateOnly, JsonElement>>() ?? [];

        return eodResults.Select(v =>
        {
            var date = v.Key;
            EodData value = v.Value.Deserialize<EodData>() ?? throw new ArgumentException();
            return new EndOfDayEntry()
            {
                Symbol = metaData?.Symbol ?? throw new ArgumentException(),
                Date = date,
                Open = value.Open,
                High = value.High,
                Low = value.Low,
                Close = value.Close,
                Volume = value.Volume
            };
        }).ToList();
    }

    public async Task<IEnumerable<StockSymbolEntry>> SearchSymbolAsync(string searchTerm, CancellationToken cancellationToken)
    {
        SearchFunctionBuilder url = new AlphaVantageUrl(_settings.BaseUrl)
                                        .Search()
                                        .ForTerm(searchTerm);

        Dictionary<string, JsonElement> result = await QueryDataAsync(url, cancellationToken).ConfigureAwait(false);

        SearchData[] data = result.ElementAt(0).Value.Deserialize<SearchData[]>() ?? [];

        return data.Select(d => new StockSymbolEntry
        {
            Key = d.Symbol,
            Name = d.Name,
            Region = d.Region,
            MarketOpen = TimeOnly.Parse(d.MarketOpen),
            MarketClose = TimeOnly.Parse(d.MarketClose),
            Type = d.Type,
            Currency = d.Currency,
            TimeZone = d.TimeZone
        }).ToList();
    }

    private async Task<Dictionary<string, JsonElement>> QueryDataAsync(FunctionQueryBuilder urlBuilder, CancellationToken cancellationToken)
    {
        Dictionary<string, JsonElement> result = [];

        using HttpClient client = new();
        string apikey = _apiKeyProvider.GetCurrentKey();
        bool retry;

        do
        {
            retry = false;
            string url = urlBuilder
                        .WithApiKey(apikey)
                        .Build();

            HttpResponseMessage response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            Stream content = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            result = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(content, cancellationToken: cancellationToken).ConfigureAwait(false) ?? [];

            if (result.TryGetValue("Information", out JsonElement json))
            {
                apikey = _apiKeyProvider.GetNewKeyAndInvalidate();
                retry = true;
            }

        } while (retry);

        return result;
    }
}
