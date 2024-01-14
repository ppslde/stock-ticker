using System.Reflection;

namespace StockTicker.WebApi.Common;

internal static class WebApplicationExtensions
{
    public static RouteGroupBuilder MapGroup(this IEndpointRouteBuilder endpoints, EndpointGroupBase group)
    {
        return endpoints
                .MapGroup($"{group.GroupName}")
                .WithTags(group.Tags.ToArray());
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        Type endpointGroupType = typeof(EndpointGroupBase);

        IEnumerable<Type> endpointGroupTypes = Assembly
                                                .GetExecutingAssembly()
                                                .GetExportedTypes()
                                                .Where(t => t.IsSubclassOf(endpointGroupType));

        foreach (var type in endpointGroupTypes)
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
                instance.Map(endpoints);

        return endpoints;
    }
}
