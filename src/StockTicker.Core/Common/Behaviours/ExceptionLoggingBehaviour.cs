using Microsoft.Extensions.Logging;

namespace StockTicker.Core.Common.Behaviours;
public class ExceptionLoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;
    private int _eventId = 0;

    public ExceptionLoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _eventId++;
            var requestName = typeof(TRequest).Name;

            Action<ILogger, string, Exception> logMessage = LoggerMessage
                .Define<string>(LogLevel.Error, new EventId(_eventId), "Unhandled Exception for Request {Name}");

            switch (ex)
            {
                case ValidationException:
                    logMessage = LoggerMessage
                        .Define<string>(LogLevel.Error, new EventId(_eventId), "Validation Exception for Request {Name}");
                    break;
                    //case NotFoundException:
                    //    logMessage = LoggerMessage
                    //    .Define<string>(LogLevel.Error, new EventId(_eventId), "Not Found Exception for Request {Name}");
                    //    break;
                    //case UnauthorizedAccessException:
                    //    logMessage = LoggerMessage
                    //    .Define<string>(LogLevel.Error, new EventId(_eventId), "Unauthorized Access Exception for Request {Name}");
                    //    break;
                    //case ForbiddenAccessException:
                    //    logMessage = LoggerMessage
                    //    .Define<string>(LogLevel.Error, new EventId(_eventId), "Forbidden Access Exception for Request {Name}");
                    //    break;
                    //case OperationFailedException:
                    //    logMessage = LoggerMessage
                    //    .Define<string>(LogLevel.Error, new EventId(_eventId), "Operation Failed Exception for Request {Name}");
                    //    break;
                    //case ConflictException:
                    //    logMessage = LoggerMessage
                    //    .Define<string>(LogLevel.Error, new EventId(_eventId), "Conflict Exception for Request {Name}");
                    //    break;
            }

            logMessage.Invoke(_logger, requestName, ex);

            throw;
        }
    }
}
