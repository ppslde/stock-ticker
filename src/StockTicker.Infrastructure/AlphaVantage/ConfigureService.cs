using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockTicker.Core.Common.Contracts;
using StockTicker.Infrastructure.AlphaVantage.Common;

namespace StockTicker.Infrastructure.AlphaVantage;
internal static class ConfigureService
{
    public static IServiceCollection AddAlphaVantageProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AlphaVantageSettings>(configuration.GetSection(nameof(AlphaVantageSettings)));

        services.AddSingleton<IApiKeyProvider, AlphaVantageApiKeyProvider>();
        services.AddTransient<IStockProvider, AlphaVantageStockProvider>();

        return services;
    }
}
