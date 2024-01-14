namespace StockTicker.Infrastructure.AlphaVantage.Common;

internal interface IApiKeyProvider
{
    string GetCurrentKey();
    string GetNewKeyAndInvalidate();
}
