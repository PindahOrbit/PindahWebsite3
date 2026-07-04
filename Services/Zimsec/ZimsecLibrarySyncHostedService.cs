namespace PindahWebsite3.Services.Zimsec;

public class ZimsecLibrarySyncHostedService : IHostedService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<ZimsecLibrarySyncHostedService> _logger;

    public ZimsecLibrarySyncHostedService(IServiceProvider services, ILogger<ZimsecLibrarySyncHostedService> logger)
    {
        _services = services;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(500, cancellationToken);
                await ZimsecDatabaseInitializer.InitializeAsync(_services);
                await ZimsecDatabaseInitializer.SyncLibraryAsync(_services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Background Zimsec library sync failed");
            }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
