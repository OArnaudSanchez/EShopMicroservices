//Configure Dependency Injection Services

//TODO: Use Extension methods => ConfigureServices class to clean this file

using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
var postgresConnectionString = builder.Configuration.GetConnectionString("BasketApi")!;
var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = builder.Configuration["RedisConfiguration:Instance"];
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter(configurator: config =>
{
    var endpointTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => typeof(ICarterModule).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
    .ToArray();

    config.WithModules(endpointTypes);
});

builder.Services.AddMarten(options =>
{
    options.Connection(postgresConnectionString);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
})
.UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString)
    .AddNpgSql(postgresConnectionString);

//Configure Request Pipeline
var app = builder.Build();
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await app.RunAsync();

public partial class Program
{ }

//TODO: API Endpoints
// HTTP GET => /basket/{userName} => Get basket with username
// HTTP POST => /basket/{userName} => Store Basket {Insert - Update}
// HTTP DELETE => /basket/{userName} => Delete basket with username
// HTTP POST => /basket/checkout => Checkout basket