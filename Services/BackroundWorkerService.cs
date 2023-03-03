namespace UpscaleTechnicalTest.Services;

public class BackroundWorkerService : BackgroundService
{
    private readonly ILogger<BackgroundService> _logger;
    private readonly IServiceProvider _services;

    public BackroundWorkerService(ILogger<BackgroundService> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }
   
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker Started at: {time}", DateTimeOffset.Now);
        return base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker Stopped at: {time}", DateTimeOffset.Now);
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                try
                {
                    var todoService = scope.ServiceProvider.GetRequiredService<ITodoService>();
                    var actionRes = await todoService.SendExpiredItemNotification();
                    if (actionRes.Error.status)
                        _logger.LogError(actionRes.Error.error);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while starting service: {Error}", ex.Message);
                }
            }

            await Task.Delay(3000, stoppingToken);
        }

        await Task.CompletedTask;
    }
}