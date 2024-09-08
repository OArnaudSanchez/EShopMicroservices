using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

namespace Catalog.API.Tests.MockHelpers
{
    public class TestEndpointRouteBuilder : IEndpointRouteBuilder
    {
        private readonly List<RouteEndpoint> _endpoints = new List<RouteEndpoint>();

        public IServiceProvider ServiceProvider { get; }

        public ICollection<EndpointDataSource> DataSources { get; } = new List<EndpointDataSource>();

        public TestEndpointRouteBuilder(IServiceProvider serviceProvider = null)
        {
            ServiceProvider = serviceProvider ?? new MockServiceProvider();
        }

        // Map for POST operation
        public void MapPost(string pattern, RequestDelegate handler)
        {
            var endpoint = new RouteEndpoint(
                handler,
                RoutePatternFactory.Parse(pattern),
                0,
                EndpointMetadataCollection.Empty,
                "TestPostEndpoint"
            );

            _endpoints.Add(endpoint);
        }

        // Map for GET operation
        public void MapGet(string pattern, RequestDelegate handler)
        {
            var endpoint = new RouteEndpoint(
                handler,
                RoutePatternFactory.Parse(pattern),
                0,
                EndpointMetadataCollection.Empty,
                "TestGetEndpoint"
            );

            _endpoints.Add(endpoint);
        }

        // Map for PUT operation
        public void MapPut(string pattern, RequestDelegate handler)
        {
            var endpoint = new RouteEndpoint(
                handler,
                RoutePatternFactory.Parse(pattern),
                0,
                EndpointMetadataCollection.Empty,
                "TestPutEndpoint"
            );

            _endpoints.Add(endpoint);
        }

        // Map for DELETE operation
        public void MapDelete(string pattern, RequestDelegate handler)
        {
            var endpoint = new RouteEndpoint(
                handler,
                RoutePatternFactory.Parse(pattern),
                0,
                EndpointMetadataCollection.Empty,
                "TestDeleteEndpoint"
            );

            _endpoints.Add(endpoint);
        }

        // Simulate invoking POST request
        public async Task<IResult> InvokePost<TRequest>(string pattern, TRequest request, ISender sender)
        {
            return await InvokeRequest(pattern, "POST", request, sender);
        }

        // Simulate invoking GET request
        public async Task<IResult> InvokeGet(string pattern, ISender sender)
        {
            return await InvokeRequest<object>(pattern, "GET", null, sender);
        }

        // Simulate invoking PUT request
        public async Task<IResult> InvokePut<TRequest>(string pattern, TRequest request, ISender sender)
        {
            return await InvokeRequest(pattern, "PUT", request, sender);
        }

        // Simulate invoking DELETE request
        public async Task<IResult> InvokeDelete(string pattern, ISender sender)
        {
            return await InvokeRequest<object>(pattern, "DELETE", null, sender);
        }

        // Generalized request invocation handler
        private async Task<IResult> InvokeRequest<TRequest>(string pattern, string method, TRequest request, ISender sender)
        {
            var endpoint = _endpoints.Find(e => e.RoutePattern.RawText == pattern);
            if (endpoint == null)
            {
                throw new InvalidOperationException($"No endpoint found for pattern: {pattern}");
            }

            // Mock HttpContext to simulate the request and response
            var httpContext = new DefaultHttpContext
            {
                RequestServices = new MockServiceProvider(sender)
            };

            httpContext.Request.Method = method;

            if (request != null)
            {
                httpContext.Items["request"] = request;
            }

            await endpoint.RequestDelegate(httpContext);

            var result = httpContext.Items["result"] as IResult;
            return result;
        }

        // Required implementation for IEndpointRouteBuilder
        public IApplicationBuilder CreateApplicationBuilder()
        {
            return new ApplicationBuilder(ServiceProvider);
        }
    }

    // MockServiceProvider for providing dependencies like ISender
    public class MockServiceProvider : IServiceProvider
    {
        private readonly ISender _sender;

        public MockServiceProvider(ISender sender = null)
        {
            _sender = sender;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ISender))
            {
                return _sender;
            }

            return null;
        }
    }
}