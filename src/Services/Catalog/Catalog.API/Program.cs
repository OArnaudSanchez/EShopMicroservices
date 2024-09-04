using BuildingBlocks.Behaviours;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

//Register DI Services - Add services to the container

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
    options.Connection(builder.Configuration.GetConnectionString("CatalogApi")!);
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

//Configure the HTTP request pipeline.
var app = builder.Build();
app.MapCarter();

app.UseExceptionHandler(options => { });

//TODO: add swagger support
await app.RunAsync();