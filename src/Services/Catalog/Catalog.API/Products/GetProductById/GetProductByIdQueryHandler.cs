//class immutable
public record GetProductByIdQuery(Guid Id):IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler(ICatalogRepository cached)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await cached.GetProductById(query.Id, cancellationToken);
        return new GetProductByIdResult(product);
    }
}
