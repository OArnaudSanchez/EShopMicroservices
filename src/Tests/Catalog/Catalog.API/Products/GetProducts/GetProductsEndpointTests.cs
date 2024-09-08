using AutoFixture;
using Catalog.API.Products.GetProducts;
using Catalog.API.Tests.MockHelpers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Catalog.API.Tests.Products.GetProducts
{
    [TestFixture]
    public class GetProductsEndpointTests
    {
        private Mock<ISender> _senderMock;

        private Fixture _fixture;

        private CancellationToken _cancellationToken;

        private GetProductsEndpoint _sut;

        [SetUp]
        public void SetUp()
        {
            _senderMock = new Mock<ISender>();
            _fixture = FixtureHelper.CreateFixtureHelper();
            _cancellationToken = new CancellationToken();
            _sut = new GetProductsEndpoint();
        }

        [Test]
        public async Task HandleGetProducts_ShouldReturn200OkAndProducts_WhenIsCalled()
        {
            // Arrange
            var productRequest = _fixture.Create<GetProductsRequest>();
            var productsResult = _fixture.Create<GetProductsResult>();

            _senderMock.Setup(x => x.Send(It.IsAny<GetProductQuery>(), _cancellationToken))
                .ReturnsAsync(productsResult);

            // Act
            var response = await _sut.HandleGetProducts(productRequest, _senderMock.Object);

            //Assert
            response.Should().BeOfType<Ok<GetProductsResponse>>();
            var result = response as Ok<GetProductsResponse>;
            result!.Value!.Products.Count().Should().Be((int)productsResult.Products.Count);
        }
    }
}