using AutoFixture;
using Catalog.API.Entities;
using Catalog.API.Products.GetProducts;
using Marten;
using Moq;

namespace Catalog.API.Tests.Products.GetProducts
{
    [TestFixture]
    public class GetProductsQueryHandlerTests
    {
        private Mock<IDocumentSession> _documentSessionMock;

        private Fixture _fixture;

        private CancellationToken _cancellationToken = CancellationToken.None;

        private GetProductsQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _fixture = new Fixture();
            _sut = new GetProductsQueryHandler(_documentSessionMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnProducts_WhenIsCalled()
        {
            //Arrange
            var query = new GetProductQuery();
            var products = _fixture.CreateMany<Product>().ToList();

            //Act
            //var result = await _sut.Handle(query, _cancellationToken);

            //Assert
        }
    }
}