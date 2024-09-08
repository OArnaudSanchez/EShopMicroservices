using AutoFixture;
using Catalog.API.Entities;
using Catalog.API.Exceptions;
using Catalog.API.Products.GetProductById;
using FluentAssertions;
using Marten;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.API.Tests.Products.GetProductById
{
    [TestFixture]
    public class GetProductByIdQueryHandlerTests
    {
        private Mock<IDocumentSession> _documentSessionMock;

        private Fixture _fixture;

        private CancellationToken _cancellationToken;

        private Mock<ILogger> _loggerMock;

        private GetProductByIdQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _fixture = new Fixture();
            _cancellationToken = _fixture.Create<CancellationToken>();
            _loggerMock = new Mock<ILogger>();
            _sut = new GetProductByIdQueryHandler(_documentSessionMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnProduct_WhenIsCalledWithExistingProductId()
        {
            //Arrange
            var product = _fixture.Create<Product>();
            var query = _fixture.Build<GetProductByIdQuery>().With(x => x.ProductId, product.Id).Create();
            _documentSessionMock.Setup(x => x.LoadAsync<Product>(query.ProductId, _cancellationToken)).ReturnsAsync(product);

            //Act
            var result = await _sut.Handle(query, _cancellationToken);

            //Assert
            result.Product.Should().Be(product);
        }

        [Test]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductIsNull()
        {
            //Arrange
            var query = _fixture.Create<GetProductByIdQuery>();

            //Act
            var result = () => _sut.Handle(query, _cancellationToken);

            //Assert
            await result.Should().ThrowAsync<ProductNotFoundException>();
            _loggerMock.Verify();
        }
    }
}