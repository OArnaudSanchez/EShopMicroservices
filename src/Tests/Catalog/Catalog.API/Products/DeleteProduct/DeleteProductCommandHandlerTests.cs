using AutoFixture;
using Catalog.API.Entities;
using Catalog.API.Exceptions;
using Catalog.API.Products.DeleteProduct;
using FluentAssertions;
using Marten;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.API.Tests.Products.DeleteProduct
{
    [TestFixture]
    public class DeleteProductCommandHandlerTests
    {
        private Mock<IDocumentSession> _documentSessionMock;

        private Fixture _fixture;

        private CancellationToken _cancellationToken;

        private Mock<ILogger> _loggerMock;

        private DeleteProductCommandHandler _sut;

        [SetUp]
        public void Setup()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _fixture = new Fixture();
            _cancellationToken = _fixture.Create<CancellationToken>();
            _loggerMock = new Mock<ILogger>();
            _sut = new DeleteProductCommandHandler(_documentSessionMock.Object);
        }

        [Test]
        public async Task Handle_ShouldDeleteProduct_WhenProductIsNotNull()
        {
            //Arrange
            var command = _fixture.Create<DeleteProductCommand>();
            var product = _fixture.Create<Product>();
            _documentSessionMock.Setup(x => x.LoadAsync<Product>(command.ProductId, _cancellationToken)).ReturnsAsync(product);

            //Act
            var result = await _sut.Handle(command, _cancellationToken);

            //Assert
            result.Should().Be(Unit.Value);
            _documentSessionMock.Verify(x => x.SaveChangesAsync(_cancellationToken), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductIsNull()
        {
            //Arrange
            var command = _fixture.Create<DeleteProductCommand>();

            //Act
            var result = () => _sut.Handle(command, _cancellationToken);

            //Assert
            await result.Should().ThrowAsync<ProductNotFoundException>();
            _loggerMock.Verify();
        }
    }
}