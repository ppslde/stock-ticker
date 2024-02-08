using StockTicker.Core;
using StockTicker.EodUpdateRunner;
using StockTicker.Infrastructure;
using StockTicker.Infrastructure.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddApplicationLogging(builder.Configuration);

builder.Services.AddStockTickerInfrastructureServices(builder.Configuration);
builder.Services.AddStockTickerCoreServices(builder.Configuration);
builder.Services.AddHostedService<EndOfDayUpdateWorker>();

IHost host = builder.Build();
await host.RunAsync();
