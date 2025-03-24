namespace TeamHub.Worker;

/// <summary>
/// Background service that runs continuously and logs a message every second.
/// </summary>
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Worker"/> class.
    /// </summary>
    /// <param name="logger">Handles logging.</param>
    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Executes the background task.
    /// </summary>
    /// <param name="stoppingToken">Token used to stop the background task.</param>
    /// <returns>A task representing the background operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
