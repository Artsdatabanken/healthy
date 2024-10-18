using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

public class HealthModel : PageModel
{
    private readonly IMemoryCache _cache;

    public HealthModel(IMemoryCache cache)
    {
        _cache = cache;
    }

    public List<KeyValuePair<string, string>> SortedHealthResults { get; private set; }

    public void OnGet()
    {
        if (_cache.TryGetValue("HealthCheckResults", out Dictionary<string, string> healthResults))
        {
            SortedHealthResults = healthResults
                .OrderBy(result => result.Value == "Healthy")
                .ThenBy(result => result.Key)
                .ToList();
        }
        else
        {
            SortedHealthResults = new List<KeyValuePair<string, string>>();
        }
    }
}
