using AutoFixture;
using Catalog.API.Products.DeleteProduct;
using Catalog.API.Products.UpdateProduct;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Catalog.API.Tests.Products.UpdateProduct
{
    [TestFixture]
    public class UpdateProductEndpointTests
    {
        private Mock<ISender> _senderMock;

        private Fixture _fixture;

        private UpdateProductEndpoint _sut;

        [SetUp]
        public void SetUp()
        {
            _senderMock = new Mock<ISender>();
            _fixture = new Fixture();
            _sut = new UpdateProductEndpoint();
        }

        [Test]
        public async Task HandleUpdateProduct_ShouldReturn204NoContent_WhenProductIsUpdatedSuccessfully()
        {
            // Arrange
            var productId = _fixture.Create<Guid>();
            var request = _fixture.Build<UpdateProductRequest>().With(x => x.Id, productId).Create();

            _senderMock.Setup(x => x.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var response = await _sut.HandleUpdateProduct(productId, request, _senderMock.Object);

            //Assert
            response.Should().BeOfType<NoContent>();
        }

        [Test]
        public async Task HandleUpdateProduct_ShouldReturn400BadRequest_WhenProductIdAndParameterIdAreNotEqual()
        {
            // Arrange
            var productId = _fixture.Create<Guid>();
            var request = _fixture.Create<UpdateProductRequest>();

            _senderMock.Setup(x => x.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var response = await _sut.HandleUpdateProduct(productId, request, _senderMock.Object);

            //Assert
            response.Should().BeOfType<BadRequest>();
        }
    }
}