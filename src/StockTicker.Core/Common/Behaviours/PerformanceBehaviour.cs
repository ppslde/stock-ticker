using System.Diagnostics;
using StockTicker.Core.Common.Contracts;

namespace StockTicker.Core.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUser _currentUserService;

    public PerformanceBehaviour(ILogger<TRequest> logger, ICurrentUser currentUserService)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        TResponse response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            string requestName = typeof(TRequest).Name;
            string userId = _currentUserService.UserId.Equals(Guid.Empty) ? "n/a" : _currentUserService.UserId.ToString();
            string userName = string.IsNullOrEmpty(_currentUserService.UserName) ? "n/a" : _currentUserService.UserName;

            _logger.LogWarning("Long running request {Name} took {ElapsedMilliseconds} ms: User: id={@UserId} name={@UserName}", requestName, elapsedMilliseconds, userId, userName);
        }

        return response;
    }
}
