using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", x =>
    {
        x.Window = TimeSpan.FromSeconds(10);
        x.PermitLimit = 5;
    });
});

var app = builder.Build();
app.UseRateLimiter();
app.MapReverseProxy();

app.Run();
