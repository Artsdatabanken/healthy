using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly IMemoryCache _cache;

    public HealthCheckController(IMemoryCache cache)
    {
        _cache = cache;
    }

    [HttpGet]
    public IActionResult GetHealthCheckResults()
    {
        if (_cache.TryGetValue("HealthCheckResults", out Dictionary<string, string> healthResults))
        {
            var response = new
            {
                Timestamp = DateTime.UtcNow,
                Results = healthResults
            };
            return Ok(response);
        }
        return NoContent();
    }

}
