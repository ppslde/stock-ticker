using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockTicker.Infrastructure.AlphaVantage.Common;

namespace StockTicker.Infrastructure.AlphaVantage;

internal class AlphaVantageApiKeyProvider : IApiKeyProvider
{
    private readonly ILogger<AlphaVantageApiKeyProvider> _logger;
    private readonly AlphaVantageSettings _settings;
    private readonly ConcurrentQueue<string> _validApiKeys = [];
    private readonly ConcurrentDictionary<DateTime, string> _expiredApiKeys = [];

    public AlphaVantageApiKeyProvider(IOptions<AlphaVantageSettings> options, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<AlphaVantageApiKeyProvider>();
        _settings = options.Value;
        _validApiKeys = new(_settings.ApiKeys);

        _logger.LogInformation("{Count} api key registerd", _validApiKeys.Count);
    }

    public string GetCurrentKey()
    {
        if (!_validApiKeys.TryPeek(out string? apikey))
            throw new KeyNotFoundException("No valid api key found");

        return apikey;
    }

    private void TryRestoreExpiredKeys()
    {
        foreach (var key in _expiredApiKeys.Where(e => DateTime.UtcNow.Date.Subtract(e.Key.Date) >= TimeSpan.FromDays(1)).ToList())
        {
            if (_expiredApiKeys.TryRemove(key))
                _validApiKeys.Enqueue(key.Value);
        }
    }

    public string GetNewKeyAndInvalidate()
    {
        _logger.LogDebug("Invalidating api key");

        if (!_validApiKeys.TryDequeue(out string? expiredKey))
            throw new KeyNotFoundException("No key for invalidation found");

        if (_expiredApiKeys.TryAdd(DateTime.UtcNow, expiredKey))
            throw new InvalidOperationException("Unable to store expired key");

        return GetCurrentKey();
    }
}