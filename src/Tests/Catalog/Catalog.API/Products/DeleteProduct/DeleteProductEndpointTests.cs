using AutoFixture;
using Catalog.API.Products.DeleteProduct;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Catalog.API.Tests.Products.DeleteProduct
{
    [TestFixture]
    public class DeleteProductEndpointTests
    {
        private Mock<ISender> _senderMock;

        private Fixture _fixture;

        private DeleteProductEndpoint _sut;

        [SetUp]
        public void SetUp()
        {
            _senderMock = new Mock<ISender>();
            _fixture = new Fixture();
            _sut = new DeleteProductEndpoint();
        }

        [Test]
        public async Task HandleDeleteProduct_ShouldReturn204NoContent_WhenProductIsDeletedSuccessfully()
        {
            // Arrange
            var productId = _fixture.Create<Guid>();

            _senderMock.Setup(x => x.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var response = await _sut.HandleDeleteProduct(productId, _senderMock.Object);

            //Assert
            response.Should().BeOfType<NoContent>();
        }
    }
}