namespace Catalog.API.Products.GetProduucts;
//class immutable
public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) :IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler
    (ICatalogRepository cached)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var productts = await cached.GetProducts(query.PageNumber??1,query.PageSize??10,cancellationToken);
        return new GetProductsResult(productts);
    }
}
