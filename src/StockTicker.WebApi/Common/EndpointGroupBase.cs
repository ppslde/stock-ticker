namespace StockTicker.WebApi.Common;

public interface IApiEndpointGroup
{
    void Map(IEndpointRouteBuilder endpoints);
}

public abstract class EndpointGroupBase : IApiEndpointGroup
{
    public abstract void Map(IEndpointRouteBuilder endpoints);
    public virtual string GroupName => GetType().Name;
    public virtual IEnumerable<string> Tags => [GroupName];
}
