namespace Catalog.API.Products.GetProductByCategory;
//class immutable
public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);
internal class GetProductByCategoryQueryHandler
    (ICatalogRepository cached)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await cached.GetProductByCategory(query.Category, cancellationToken);
        return new GetProductByCategoryResult(products);
    }
}
