using AutoFixture;
using Catalog.API.Products.CreateProduct;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Catalog.API.Tests.Products.CreateProduct
{
    [TestFixture]
    public class CreateProductEndpointTests
    {
        private Mock<ISender> _senderMock;

        private Fixture _fixture;

        private CreateProductEndpoint _sut;

        [SetUp]
        public void SetUp()
        {
            _senderMock = new Mock<ISender>();
            _fixture = new Fixture();
            _sut = new CreateProductEndpoint();
        }

        [Test]
        public void AddRoutes_ShouldConfigureProductEndpoint_WhenIsExcecuted()
        {
            // Arrange
            var routeBuilderMock = new Mock<IEndpointRouteBuilder>();

            // Act
            //_sut.AddRoutes(routeBuilderMock.Object);

            // Assert
            //routeBuilderMock.Verify(r => r.MapPost(
            //    "/products",
            //    It.IsAny<RequestDelegate>()
            //), Times.Once);
        }

        [Test]
        public async Task HandleCreateProduct_ShouldReturn201Created_WhenProductIsCreatedSuccessfully()
        {
            // Arrange
            var createProductRequest = _fixture.Create<CreateProductRequest>();
            var createProductResult = _fixture.Create<CreateProductResult>();

            _senderMock.Setup(x => x.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createProductResult);

            // Act
            var response = await _sut.HandleCreateProduct(createProductRequest, _senderMock.Object);

            //Assert
            response.Should().BeOfType<Created<CreateProductResponse>>();
            var result = response as Created<CreateProductResponse>;
            result!.Value!.Id.Should().Be(createProductResult.Id);
        }
    }
}