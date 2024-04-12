using Catalog.API.Products.DeleteProduct;

namespace Catalog.API.Products.DeleteProduct
{
    //class immutable
    public record DeleteProductCommand(Guid Id):ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
        }
    }
    internal class DeleteProductHandler(IDocumentSession session)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand query, CancellationToken cancellationToken)
        {
            //Delete Product entity from command object
            var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }
            //Delete to database
            session.Delete(product);
            //time out
            await session.SaveChangesAsync(cancellationToken);
            //return DeleteProductResult result
            return new DeleteProductResult(true);
        }
    }
}
