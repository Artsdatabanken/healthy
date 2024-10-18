using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

public class HealthModel : PageModel
{
    private readonly IMemoryCache _cache;
    private readonly TimeZoneInfo _timeZone;

    public HealthModel(IMemoryCache cache)
    {
        _cache = cache;
        _timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
    }

    public List<KeyValuePair<string, string>> SortedHealthResults { get; private set; }
    public DateTime LastChecked { get; private set; }

    public void OnGet()
    {
        if (_cache.TryGetValue("HealthCheckResults", out Dictionary<string, string> healthResults))
        {
            SortedHealthResults = healthResults
                .OrderBy(result => result.Value == "Healthy")
                .ThenBy(result => result.Key)
                .ToList();

            var utcNow = DateTime.UtcNow;
            LastChecked = TimeZoneInfo.ConvertTimeFromUtc(utcNow, _timeZone);
        }
        else
        {
            SortedHealthResults = new List<KeyValuePair<string, string>>();
            LastChecked = DateTime.MinValue;
        }
    }
}
