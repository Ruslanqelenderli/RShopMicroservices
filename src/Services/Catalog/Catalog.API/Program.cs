using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Data;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCarter();
builder.Services.AddMarten(x => x.Connection(builder.Configuration.GetConnectionString("Database")!)).UseLightweightSessions();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});


builder.Services.AddValidatorsFromAssembly(assembly);


if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();

app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.MapCarter();
app.UseExceptionHandler(options => { });
app.Run();
