using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using StockTicker.Core.Common.Contracts;
using StockTicker.WebApi.Services;

namespace StockTicker.WebApi;

internal static class DependencyInjection
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUserService>();

        services.AddFluentValidationClientsideAdapters();
        services.AddHealthChecks();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
        });

        return services;
    }
}
