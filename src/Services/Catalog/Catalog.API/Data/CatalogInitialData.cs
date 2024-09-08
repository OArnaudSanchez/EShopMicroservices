using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync())
            {
                return;
            }

            var products = GetPreconfiguredProducts();
            session.Store(products);
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
            {
                new Product(
                    new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                    "IPhone X",
                    new List<string> { "Smart Phone" },
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    "product-1.png",
                    950.00M
                ),
                new Product(
                    new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
                    "Samsung 10",
                    new List<string> { "Smart Phone" },
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    "product-2.png",
                    840.00M
                ),
                new Product(
                    new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
                    "Huawei Plus",
                    new List<string> { "White Appliances" },
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    "product-3.png",
                    650.00M
                ),
                new Product(
                    new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
                    "Xiaomi Mi 9",
                    new List<string> { "White Appliances" },
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    "product-4.png",
                    470.00M
                ),
                new Product(
                    new Guid("b786103d-c621-4f5a-b498-23452610f88c"),
                    "HTC U11+ Plus",
                    new List<string> { "Smart Phone" },
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    "product-5.png",
                    380.00M
                ),
                new Product(
                    new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
                    "LG G7 ThinQ",
                    new List<string> { "Home Kitchen" },
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    "product-6.png",
                    240.00M
                ),
                new Product(
                    new Guid("93170c85-7795-489c-8e8f-7dcf3b4f4188"),
                    "Panasonic Lumix",
                    new List<string> { "Camera" },
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    "product-6.png",
                    240.00M
                )
            };
    }
}