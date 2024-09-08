using AutoFixture;
using Catalog.API.Products.GetProductByCategory;
using Marten;
using Moq;

namespace Catalog.API.Tests.Products.GetProductByCategory
{
    [TestFixture]
    public class GetProductByCategoryQueryHandlerTests
    {
        private Mock<IDocumentSession> _documentSessionMock;

        private Fixture _fixture;

        private CancellationToken _cancellationToken;

        private GetProductByCategoryQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _fixture = new Fixture();
            _cancellationToken = _fixture.Create<CancellationToken>();
            _sut = new GetProductByCategoryQueryHandler(_documentSessionMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnProductsByCategory_WhenIsCalledWithAnExistingCategory()
        {
            // Arrange
            var query = _fixture.Create<GetProductByCategoryQuery>();

            // Act
            //var result = await _sut.Handle(query, _cancellationToken);

            // Assert
        }
    }
}