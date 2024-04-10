namespace Catalog.API.Data
{
    public interface ICatalogRepository
    {
        Task<IEnumerable<Product>> GetProductByCategory(string Category, CancellationToken cancellationToken = default);
        Task<Product> GetProductById(Guid Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetProducts(int PageNumber = 1, int PageSize = 10, CancellationToken cancellationToken = default);
    }
}
