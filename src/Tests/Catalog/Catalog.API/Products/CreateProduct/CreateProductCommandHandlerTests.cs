using AutoFixture;
using FluentAssertions;
using Marten;
using Moq;

namespace Catalog.API.Products.CreateProduct
{
    [TestFixture]
    public class CreateProductCommandHandlerTests
    {
        private Mock<IDocumentSession> _documentSessionMock;

        private Fixture _fixture;

        private CancellationToken _cancellationToken;

        private CreateProductCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _documentSessionMock = new Mock<IDocumentSession>();
            _fixture = new Fixture();
            _cancellationToken = new CancellationToken();
            _sut = new CreateProductCommandHandler(_documentSessionMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateProduct_WhenIsCalledWithValidProduct()
        {
            //Arrange
            var command = _fixture.Create<CreateProductCommand>();

            //Act
            var result = await _sut.Handle(command, _cancellationToken);

            //Assert
            result.Should().NotBeNull();
        }
    }
}