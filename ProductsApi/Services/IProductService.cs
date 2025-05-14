using ProductsApi.Models;
using ProductsApi.ProductDTOs;

namespace ProductsApi.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> DecrementStockAsync(int id, int quantity);
        Task<bool> IncrementStockAsync(int id, int quantity);
    }
}
