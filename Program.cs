var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<HealthCheckService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Read hosts from configuration
var healthCheckHosts = builder.Configuration.GetSection("HealthCheckHosts").Get<string[]>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHealthChecks("/health")
    .RequireHost(healthCheckHosts);

app.Run();
