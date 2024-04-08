namespace Catalog.API.Products.UpdateProduct
{
    //class immutable
    public record UpdateProductCommand(
        Guid Id,
        string Name, 
        List<string> Category,
        string Description,
        string ImageFile,
        decimal Price)
        :ICommand<UpdateProductResult>;
    public record UpdateProductResult(Product Product);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2,150).WithMessage("Name must be between 2 and 150 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater then 0");
        }
    }

    internal class UpdateProductHandler(IDocumentSession session)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand query, CancellationToken cancellationToken)
        {
            //update Product entity from command object
            var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }

            product.Name = query.Name;
            product.Category = query.Category;
            product.Description = query.Description;
            product.ImageFile = query.ImageFile;
            product.Price = query.Price;
            //update to database
            session.Update(product);
            //time out
            await session.SaveChangesAsync(cancellationToken);
            //return UpdateProductResult result
            return new UpdateProductResult(product);
        }
    }
}
