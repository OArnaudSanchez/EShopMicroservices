using Marten;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Catalog.API.Tests.MockHelpers
{
    internal class WebFactoryHelper(string environment = "Development") : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(environment);

            // Add mock/test services to the builder here
            builder.ConfigureServices(services =>
            {
                services.AddMarten(options =>
                {
                    options.Connection("Host=localhost;Database=testdb;Username=test;Password=test");
                    options.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;
                });
            });

            return base.CreateHost(builder);
        }
    }
}