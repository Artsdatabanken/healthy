public class HealthCheckService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HealthCheckService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Dictionary<string, string>> CheckHostsAsync(IEnumerable<string> hosts)
    {
        var results = new Dictionary<string, string>();
        var client = _httpClientFactory.CreateClient();

        foreach (var host in hosts)
        {
            try
            {
                var response = await client.GetAsync($"https://{host}");
                results[host] = response.IsSuccessStatusCode ? "Healthy" : "Unhealthy";
            }
            catch
            {
                results[host] = "Unreachable";
            }
        }

        return results;
    }
}
//healthy