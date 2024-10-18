using Microsoft.AspNetCore.Mvc.RazorPages;
public class HealthModel : PageModel
{
    private readonly HealthCheckService _healthCheckService;
    private readonly IConfiguration _configuration;

    public HealthModel(HealthCheckService healthCheckService, IConfiguration configuration)
    {
        _healthCheckService = healthCheckService;
        _configuration = configuration;
    }

    public List<KeyValuePair<string, string>> SortedHealthResults { get; private set; }

    public async Task OnGetAsync()
    {
        var hosts = _configuration.GetSection("HealthCheckHosts").Get<string[]>();
        var healthResults = await _healthCheckService.CheckHostsAsync(hosts);

        // Sort the results: unhealthy first, then by hostname alphabetically
        SortedHealthResults = healthResults
            .OrderBy(result => result.Value == "Healthy")
            .ThenBy(result => result.Key)
            .ToList();
    }
}
