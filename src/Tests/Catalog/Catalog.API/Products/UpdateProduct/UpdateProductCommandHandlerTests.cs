using AutoFixture;
using Catalog.API.Entities;
using Catalog.API.Exceptions;
using Catalog.API.Products.UpdateProduct;
using FluentAssertions;
using Marten;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.API.Tests.Products.UpdateProduct
{
    [TestFixture]
    public class UpdateProductCommandHandlerTests
    {
        private Mock<IDocumentSession> _documentSessionMock;

        private Fixture _fixture;

        private UpdateProductCommandHandler _sut;

        private Mock<ILogger> _loggerMock;

        private CancellationToken _cancellationToken;

        [SetUp]
        public void Setup()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger>();
            _cancellationToken = _fixture.Create<CancellationToken>();
            _sut = new UpdateProductCommandHandler(_documentSessionMock.Object);
        }

        [Test]
        public async Task Handle_ShouldUpdateProduct_WhenProductIsNotNull()
        {
            //Arrange
            var command = _fixture.Create<UpdateProductCommand>();
            var product = _fixture.Create<Product>();
            _documentSessionMock.Setup(x => x.LoadAsync<Product>(command.Id, _cancellationToken)).ReturnsAsync(product);

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
            var command = _fixture.Create<UpdateProductCommand>();

            //Act
            var result = () => _sut.Handle(command, _cancellationToken);

            //Assert
            await result.Should().ThrowAsync<ProductNotFoundException>();
            _loggerMock.Verify();
        }
    }
}