using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Order.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            //MediatR
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);
                config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            });

            //FluentValidation
            services.AddValidatorsFromAssembly(assembly);

            //Mappings
            return services;
        }
    }
}