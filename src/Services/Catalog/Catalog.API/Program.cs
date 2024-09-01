using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//Register DI Services - Add services to the container
builder.Services.AddCarter(configurator: config =>
{
    var endpointTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => typeof(ICarterModule).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
    .ToArray();

    config.WithModules(endpointTypes);
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogApi")!);
}).UseLightweightSessions();

//Configure the HTTP request pipeline.
var app = builder.Build();
app.MapCarter();

await app.RunAsync();