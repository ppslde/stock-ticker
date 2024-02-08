using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace StockTicker.Infrastructure.Logging;
public static class LoggingConfigurationExtensions
{
    public static ILoggingBuilder AddApplicationLogging(this ILoggingBuilder builder, IConfiguration configuration)
    {
        builder.ClearProviders();
        Logger l = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();
        builder.AddSerilog(l);

        return builder;
    }
}
