﻿namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand
       (string Name,
       List<string> Categories,
       string Description,
       string ImageFile,
       decimal Price) : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = Product.Create(command.Name, command.Categories, command.Description, command.ImageFile, command.Price);

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }
    }
}