using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockTicker.Core.Common.Contracts;
using StockTicker.Infrastructure.AlphaVantage;
using StockTicker.Infrastructure.Data;
using StockTicker.Infrastructure.Storage;

namespace StockTicker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddStockTickerInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IStockSymbolRepository, StockSymbolRepository>();
        services.AddTransient<IEndOfDayValuesRepository, EndOfDayValuesRepository>();

        services.AddAlphaVantageProvider(configuration);
        services.AddAzureTableStorage(configuration);

        return services;
    }
}
