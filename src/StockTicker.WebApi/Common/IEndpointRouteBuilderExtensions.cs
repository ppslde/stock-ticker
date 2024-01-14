namespace StockTicker.WebApi.Common;

internal static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, string pattern = "")
    {
        handler.AssertAnonymousMethod();

        builder
            .MapGet(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler, string pattern = "")
    {
        handler.AssertAnonymousMethod();

        builder
            .MapPost(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapPostAnonymous(this IEndpointRouteBuilder builder, Delegate handler, string pattern = "")
    {
        handler.AssertAnonymousMethod();

        builder
            .MapPost(pattern, handler)
            .AllowAnonymous()
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, string pattern)
    {
        handler.AssertAnonymousMethod();

        builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler, string pattern)
    {
        handler.AssertAnonymousMethod();

        builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }
}
