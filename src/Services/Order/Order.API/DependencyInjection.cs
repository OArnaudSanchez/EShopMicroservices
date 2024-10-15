using BuildingBlocks.Exceptions.Handler;
using Carter;
using Order.Infrastructure.Extensions;
using System.Reflection;

namespace Order.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCarter(configurator: config =>
            {
                var endpointTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(ICarterModule).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                .ToArray();

                config.WithModules(endpointTypes);
            });

            services.AddExceptionHandler<CustomExceptionHandler>();
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();

            app.UseExceptionHandler(options => { });

            if (app.Environment.IsDevelopment())
            {
                app.InitializeDatabaseAsync().GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            return app;
        }
    }
}