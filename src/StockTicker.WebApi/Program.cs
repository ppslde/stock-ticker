using StockTicker.Core;
using StockTicker.Infrastructure;
using StockTicker.Infrastructure.Logging;
using StockTicker.WebApi;
using StockTicker.WebApi.Common;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddStockTickerCoreServices(builder.Configuration);
builder.Services.AddStockTickerInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

builder.Logging.AddApplicationLogging(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddRouting(options => options.LowercaseUrls = true);


var app = builder.Build();

app.UseHealthChecks("/healthy");

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapEndpoints();
    //endpoints.MapControllers();
    endpoints.MapSwagger();
});
#pragma warning restore ASP0014

await app.RunAsync().ConfigureAwait(false);