using AutoFixture;
using AutoFixture.Kernel;
using Catalog.API.Entities;
using Marten.Pagination;
using System.Collections;

namespace Catalog.API.Tests.MockHelpers
{
    public static class FixtureHelper
    {
        public static Fixture CreateFixtureHelper()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new ProductSpecimenBuilderHelper());
            fixture.Customizations.Add(new PagedListSpecimenBuilderHelper(fixture));
            return fixture;
        }
    }

    public class ProductSpecimenBuilderHelper : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type requestType && requestType == typeof(Product))
            {
                return new Product(
                    new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                    "IPhone X",
                    new List<string> { "Smart Phone" },
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    "product-1.png",
                    950.00M
                );
            }

            return new NoSpecimen();
        }
    }

    public class PagedListSpecimenBuilderHelper : ISpecimenBuilder
    {
        private readonly Fixture _fixture;

        public PagedListSpecimenBuilderHelper(Fixture fixture)
        {
            _fixture = fixture;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type requestType && requestType.IsGenericType &&
                requestType.GetGenericTypeDefinition() == typeof(IPagedList<>) &&
                requestType.GetGenericArguments().First() == typeof(Product))
            {
                // Create a sample paged list with random products
                var products = _fixture.CreateMany<Product>(10).ToList();
                return new PagedList<Product>(products, products.Count, 1, 10);
            }

            return new NoSpecimen();
        }
    }

    public class PagedList<T>(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize) : IPagedList<T>
    {
        public T this[int index] => throw new NotImplementedException();

        public IEnumerable<T> Items { get; } = items;

        public int TotalCount { get; } = totalCount;

        public int PageNumber { get; } = pageNumber;

        public int PageSize { get; } = pageSize;

        public long Count => totalCount;

        public long PageCount => throw new NotImplementedException();

        public long TotalItemCount => throw new NotImplementedException();

        public bool HasPreviousPage => throw new NotImplementedException();

        public bool HasNextPage => throw new NotImplementedException();

        public bool IsFirstPage => throw new NotImplementedException();

        public bool IsLastPage => throw new NotImplementedException();

        public long FirstItemOnPage => throw new NotImplementedException();

        public long LastItemOnPage => throw new NotImplementedException();

        long IPagedList<T>.PageNumber => PageNumber;

        long IPagedList<T>.PageSize => PageSize;

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}