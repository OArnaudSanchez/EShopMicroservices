namespace Order.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            //TODO: Carter configuration

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            //Use Swagger configuration

            return app;
        }
    }
}