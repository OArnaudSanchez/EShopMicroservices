using AutoFixture;
using Catalog.API.Entities;
using Catalog.API.Products.GetProductByCategory;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Catalog.API.Tests.Products.GetProductByCategory
{
    [TestFixture]
    public class GetProductByCategoryEndpointTests
    {
        private Mock<ISender> _senderMock;

        private Fixture _fixture;

        private GetProductByCategoryEndpoint _sut;

        [SetUp]
        public void SetUp()
        {
            _senderMock = new Mock<ISender>();
            _fixture = new Fixture();
            _sut = new GetProductByCategoryEndpoint();
        }

        [Test]
        public async Task HandleGetProductByCategory_ShouldReturn200OkAndProducts_WhenIsCalled()
        {
            // Arrange
            var category = _fixture.Create<string>();
            var products = _fixture.CreateMany<Product>().ToList();
            var productsResult = _fixture.Build<GetProductByCategoryResult>().With(x => x.Products, products).Create();
            _senderMock.Setup(x => x.Send(It.IsAny<GetProductByCategoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productsResult);

            // Act
            var response = await _sut.HandleGetProductByCategory(category, _senderMock.Object);

            //Assert
            response.Should().BeOfType<Ok<IEnumerable<Product>>>();
            var result = response as Ok<IEnumerable<Product>>;
            result!.Value!.Count().Should().Be(products.Count);
        }
    }
}