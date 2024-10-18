using Microsoft.Extensions.Caching.Memory;

public class HealthCheckBackgroundService : BackgroundService
{
    private readonly HealthCheckService _healthCheckService;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;

    public HealthCheckBackgroundService(HealthCheckService healthCheckService, IConfiguration configuration, IMemoryCache cache)
    {
        _healthCheckService = healthCheckService;
        _configuration = configuration;
        _cache = cache;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var hosts = _configuration.GetSection("HealthCheckHosts").Get<string[]>();
            var healthResults = await _healthCheckService.CheckHostsAsync(hosts);

            _cache.Set("HealthCheckResults", healthResults, TimeSpan.FromMinutes(5));

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);

        }
    }
}
