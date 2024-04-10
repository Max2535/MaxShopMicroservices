
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace Catalog.API.Data
{
    public class CachedCatalogRepository(ICatalogRepository repository, IDistributedCache cache) : ICatalogRepository
    {
        public async Task<IEnumerable<Product>> GetProductByCategory(string Category, CancellationToken cancellationToken = default)
        {
            string key = Category;
            var cachedProduct = await cache.GetStringAsync(key, cancellationToken);
            if (!string.IsNullOrEmpty(cachedProduct))
                return JsonSerializer.Deserialize<IEnumerable<Product>>(cachedProduct)!;

            var products = await repository.GetProductByCategory(Category, cancellationToken);
            await cache.SetStringAsync(key, JsonSerializer.Serialize(products), new DistributedCacheEntryOptions
            {
                //TODO:: get time from config
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });
            return products;
        }

        public async Task<Product> GetProductById(Guid Id, CancellationToken cancellationToken = default)
        {
            string key = Id.ToString();
            var cachedProduct = await cache.GetStringAsync(key, cancellationToken);
            if (!string.IsNullOrEmpty(cachedProduct))
                return JsonSerializer.Deserialize<Product>(cachedProduct)!;

            var product = await repository.GetProductById(Id, cancellationToken);
            await cache.SetStringAsync(key, JsonSerializer.Serialize(product), new DistributedCacheEntryOptions
            {
                //TODO:: get time from config
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });
            return product;
        }

        public async Task<IEnumerable<Product>> GetProducts(int PageNumber = 1, int PageSize = 10, CancellationToken cancellationToken = default)
        {
            string key = $"PageNumber={PageNumber}&PageSize={PageSize}";
            var cachedProduct = await cache.GetStringAsync(key, cancellationToken);
            if (!string.IsNullOrEmpty(cachedProduct))
                return JsonSerializer.Deserialize<IEnumerable<Product>>(cachedProduct)!;

            var products = await repository.GetProducts(PageNumber,PageSize, cancellationToken);
            await cache.SetStringAsync(key, JsonSerializer.Serialize(products), new DistributedCacheEntryOptions
            {
                //TODO:: get time from config
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });
            return products;
        }
    }
}
