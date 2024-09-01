namespace Catalog.API.Entities
{
    public class Product
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; private set; }

        public List<string> Categories { get; private set; }

        public string Description { get; private set; }

        public string ImageFile { get; private set; }

        public decimal Price { get; private set; }

        // Constructor to initialize all properties
        public Product(Guid id, string name, List<string> categories, string description, string imageFile, decimal price)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Categories = categories ?? throw new ArgumentNullException(nameof(categories));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            ImageFile = imageFile ?? throw new ArgumentNullException(nameof(imageFile));
            Price = price;
        }

        public static Product Create(
            string name,
            List<string> categories,
            string description,
            string imageFile,
            decimal price)
        {
            Product product = new()
            {
                Name = name,
                Categories = categories,
                Description = description,
                ImageFile = imageFile,
                Price = price
            };
            //TODO: We can send event notifications when these events occcurs
            return product;
        }

        public static Product Update(
            Product product,
            string name,
            List<string> categories,
            string description,
            string imageFile,
            decimal price)
        {
            product.Name = name ?? product.Name;
            product.Categories = categories.Count > 0 ? categories : product.Categories;
            product.Description = description ?? product.Description;
            product.Price = price > 0 ? price : product.Price;
            return product;
        }

        // This parameterless constructor is still needed for the ORM
        private Product()
        { }
    }
}