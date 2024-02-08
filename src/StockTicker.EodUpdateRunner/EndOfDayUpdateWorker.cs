using MediatR;
using StockTicker.Core.DailyData;

namespace StockTicker.EodUpdateRunner;

public class EndOfDayUpdateWorker : IHostedService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ISender _sender;
    private readonly ILogger _logger;

    public EndOfDayUpdateWorker(IHostApplicationLifetime lifetime, ISender sender, ILoggerFactory loggerFactory)
    {
        _lifetime = lifetime;
        _sender = sender;
        _logger = loggerFactory.CreateLogger<EndOfDayUpdateWorker>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {


        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        }
        await Task.Delay(1000, cancellationToken);

        await _sender.Send(new UpdateEndOfDayEnabledTickerCommand(), cancellationToken);

        _lifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
