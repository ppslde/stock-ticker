using System.Reflection;
using StockTicker.Core.Common.Contracts;
using StockTicker.Core.Common.Security;

namespace StockTicker.Core.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
{
    private readonly ICurrentUser _currentUser;

    public AuthorizationBehaviour(ICurrentUser currentUserService)
    {
        _currentUser = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        AuthorizeAttribute? authorizeAttribute = request.GetType().GetCustomAttribute<AuthorizeAttribute>();

        if (authorizeAttribute is not null)
        {
            // do the security stuff here

        }

        // User is authorized / authorization not required
        return await next();
    }
}
