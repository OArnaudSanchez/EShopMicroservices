var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
var connectionString = builder.Configuration.GetConnectionString("CatalogApi")!;

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
    options.Connection(connectionString);
})
.UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    //builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);

//Configure the HTTP request pipeline.
var app = builder.Build();
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

//TODO: add swagger support
await app.RunAsync();