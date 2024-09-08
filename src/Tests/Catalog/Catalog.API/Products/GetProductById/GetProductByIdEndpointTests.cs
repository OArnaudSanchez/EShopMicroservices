using AutoFixture;
using Catalog.API.Products.GetProductById;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Catalog.API.Tests.Products.GetProductById
{
    [TestFixture]
    public class GetProductByIdEndpointTests
    {
        private Mock<ISender> _senderMock;

        private Fixture _fixture;

        private GetProductByIdEndpoint _sut;

        [SetUp]
        public void SetUp()
        {
            _senderMock = new Mock<ISender>();
            _fixture = new Fixture();
            _sut = new GetProductByIdEndpoint();
        }

        [Test]
        public async Task HandleGetProductById_ShouldReturn200OkAndProduct_WhenIsCalledWithProductId()
        {
            // Arrange
            var productId = _fixture.Create<Guid>();
            var productResult = _fixture.Create<GetProductByIdResult>();

            _senderMock.Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productResult);

            // Act
            var response = await _sut.HandleGetProductById(productId, _senderMock.Object);

            //Assert
            response.Should().BeOfType<Ok<GetProductResponse>>();
            var result = response as Ok<GetProductResponse>;
            result!.Value!.Product.Id.Should().Be(productResult.Product.Id);
        }
    }
}