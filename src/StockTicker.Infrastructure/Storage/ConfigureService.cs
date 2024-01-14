using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockTicker.Infrastructure.Storage.Common;

namespace StockTicker.Infrastructure.Storage;

internal static class ConfigureService
{
    public static IServiceCollection AddAzureTableStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzTableStorageSettings>(configuration.GetSection(nameof(AzTableStorageSettings)));

        services.AddTransient(typeof(IStorage<>), typeof(AzTableStorageService<>));

        return services;
    }
}
