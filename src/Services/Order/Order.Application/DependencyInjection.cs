using Microsoft.Extensions.DependencyInjection;

namespace Order.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //TODO: Add Application DI services

            //MediatR
            //FluentValidation
            //Mappings
            return services;
        }
    }
}