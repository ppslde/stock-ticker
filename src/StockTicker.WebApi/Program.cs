using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ppsl.Serilog.Logging;
using Serilog;
using StockTicker.Core;
using StockTicker.Infrastructure;
using StockTicker.WebApi;
using StockTicker.WebApi.Common;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddStockTickerCoreServices(builder.Configuration);
builder.Services.AddStockTickerInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = builder.Configuration["AllowedOrigins"]?.Split(";");

        if (origins == null || origins.Length == 0)
            policy.AllowAnyOrigin();
        else
            policy.WithOrigins(origins);

        policy
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

builder.AddLogging("stock.ticker.api..log");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseCors();
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