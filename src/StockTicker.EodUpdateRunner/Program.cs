using Serilog;
using Serilog.Core;
using StockTicker.Core;
using StockTicker.EodUpdateRunner;
using StockTicker.Infrastructure;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
Logger l = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
builder.Logging.AddSerilog(l);

builder.Services.AddStockTickerInfrastructureServices(builder.Configuration);
builder.Services.AddStockTickerCoreServices(builder.Configuration);
builder.Services.AddHostedService<EndOfDayUpdateWorker>();

IHost host = builder.Build();
await host.RunAsync();
