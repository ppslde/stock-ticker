using MediatR.Pipeline;
using StockTicker.Core.Common.Contracts;

namespace StockTicker.Core.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUser _currentUserService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUser currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        Guid userId = _currentUserService.UserId;
        string userName = _currentUserService.UserName;

        _logger.LogInformation("Request: {Name} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);

        return Task.CompletedTask;
    }
}
