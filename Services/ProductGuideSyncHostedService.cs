using Microsoft.Extensions.Options;

namespace PindahWebsite3.Services;

/// <summary>
/// Continuously keeps wwwroot/product-guides in sync with the Operations product-documentation folder.
/// Uses an interval poll plus a file-system watcher when the source is available.
/// </summary>
public class ProductGuideSyncHostedService : BackgroundService
{
    private readonly ProductGuideSyncService _sync;
    private readonly IOptionsMonitor<ProductGuidesOptions> _options;
    private readonly ILogger<ProductGuideSyncHostedService> _logger;
    private FileSystemWatcher? _watcher;
    private readonly object _watchLock = new();
    private DateTime _lastTriggeredUtc = DateTime.MinValue;

    public ProductGuideSyncHostedService(
        ProductGuideSyncService sync,
        IOptionsMonitor<ProductGuidesOptions> options,
        ILogger<ProductGuideSyncHostedService> logger)
    {
        _sync = sync;
        _options = options;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Let the site finish starting before first copy.
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _sync.SyncOnce();
                EnsureWatcher();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Product guide sync failed");
            }

            var seconds = Math.Clamp(_options.CurrentValue.SyncIntervalSeconds, 5, 3600);
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(seconds), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    public override void Dispose()
    {
        DisposeWatcher();
        base.Dispose();
    }

    private void EnsureWatcher()
    {
        if (!_options.CurrentValue.Enabled)
        {
            DisposeWatcher();
            return;
        }

        var source = _sync.ResolveSourcePath();
        if (string.IsNullOrWhiteSpace(source) || !Directory.Exists(source))
        {
            DisposeWatcher();
            return;
        }

        lock (_watchLock)
        {
            if (_watcher != null
                && string.Equals(_watcher.Path, source, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            DisposeWatcher();

            try
            {
                var watcher = new FileSystemWatcher(source)
                {
                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.FileName
                        | NotifyFilters.DirectoryName
                        | NotifyFilters.LastWrite
                        | NotifyFilters.Size,
                    Filter = "*.*"
                };

                watcher.Changed += OnSourceChanged;
                watcher.Created += OnSourceChanged;
                watcher.Renamed += OnSourceChanged;
                watcher.Deleted += OnSourceChanged;
                watcher.EnableRaisingEvents = true;
                _watcher = watcher;
                _logger.LogInformation("Watching product guide source: {Source}", source);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not watch product guide source {Source}", source);
            }
        }
    }

    private void OnSourceChanged(object sender, FileSystemEventArgs e)
    {
        // Debounce bursty editor/save events.
        var now = DateTime.UtcNow;
        if ((now - _lastTriggeredUtc).TotalMilliseconds < 750)
        {
            return;
        }

        _lastTriggeredUtc = now;

        try
        {
            _sync.SyncOnce();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Product guide watcher sync failed after {Change}", e.FullPath);
        }
    }

    private void DisposeWatcher()
    {
        lock (_watchLock)
        {
            if (_watcher == null)
            {
                return;
            }

            _watcher.EnableRaisingEvents = false;
            _watcher.Changed -= OnSourceChanged;
            _watcher.Created -= OnSourceChanged;
            _watcher.Renamed -= OnSourceChanged;
            _watcher.Deleted -= OnSourceChanged;
            _watcher.Dispose();
            _watcher = null;
        }
    }
}
