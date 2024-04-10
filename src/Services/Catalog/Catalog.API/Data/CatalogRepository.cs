
namespace Catalog.API.Data;

public class CatalogRepository(IDocumentSession session) : ICatalogRepository
{
    public async Task<IEnumerable<Product>> GetProductByCategory(string Category, CancellationToken cancellationToken = default)
    {
        var products = await session.Query<Product>()
             .Where(p => p.Category.Contains(Category))
             .ToListAsync(cancellationToken);
        return products;
    }

    public async Task<Product> GetProductById(Guid Id, CancellationToken cancellationToken = default)
    {
        var product = await session.LoadAsync<Product>(Id, cancellationToken);
        if (product is null)
        {
            throw new ProductNotFoundException();
        }
        return product;
    }

    public async Task<IEnumerable<Product>> GetProducts(int PageNumber = 1, int PageSize = 10, CancellationToken cancellationToken = default)
    {
        var productts = await session.Query<Product>()
            .ToPagedListAsync(PageNumber, PageSize, cancellationToken);
        return productts;
    }
}
