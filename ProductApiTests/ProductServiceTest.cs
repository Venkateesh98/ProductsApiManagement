using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;
using ProductsApi.Services;
using Xunit;
using FluentAssertions;
using ProductsApi.ProductDTOs;

namespace ProductApiTests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateProduct_ShouldAddProduct()
        {
            // Arrange
            var context = TestDbContextFactory.CreateDbContext();
            var service = new ProductService(context);
            var product = new Product
            {
                Name = "Test Product1",
                Description = "A test product1",
                Price = 10.0m,
                Stock = 50
            };

            // Act
            var createdProduct = await service.CreateProductAsync(product);

            // Assert
            createdProduct.Should().NotBeNull();
            createdProduct.ProductId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_IfExists()
        {
            // Arrange
            var context = TestDbContextFactory.CreateDbContext();
            var service = new ProductService(context);
            var product = new Product
            {
                Name = "Existing Product",
                Description = "A existing product1",
                Price = 20.0m,
                Stock = 100
            };
            var createdProduct = await service.CreateProductAsync(product);

            // Act
            var fetchedProduct = await service.GetProductByIdAsync(createdProduct.ProductId);

            // Assert
            fetchedProduct.Should().NotBeNull();
            fetchedProduct.Name.Should().Be("Existing Product");
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNull_IfNotExists()
        {
            // Arrange
            var context = TestDbContextFactory.CreateDbContext();
            var service = new ProductService(context);

            // Act
            var fetchedProduct = await service.GetProductByIdAsync(999); // Non-existing ID

            // Assert
            fetchedProduct.Should().BeNull();
        }
        [Fact]
        public async Task UpdateProduct_ShouldUpdateExistingProduct()
        {
            // Arrange: first, create the product directly using the service/test context.
            var context = TestDbContextFactory.CreateDbContext();
            var service = new ProductService(context);
            var createdProduct = await service.CreateProductAsync(new Product
            {
                Name = "Initial Name",
                Description = "Initial Description",
                Price = 100,
                Stock = 20
            });

            // Act: simulate an update via the controller (or call the service directly)
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Name",
                Description = "Updated Description",
                Price = 150,
                Stock = 30
            };

            // Emulate what the controller does:
            createdProduct.Name = updateDto.Name;
            createdProduct.Description = updateDto.Description;
            createdProduct.Price = updateDto.Price;
            createdProduct.Stock = updateDto.Stock;
            createdProduct.UpdatedAt = System.DateTime.UtcNow;

            var result = await service.UpdateProductAsync(createdProduct);

            // Assert that the product got updated.
            result.Should().BeTrue();
            var updatedProduct = await service.GetProductByIdAsync(createdProduct.ProductId);
            updatedProduct.Name.Should().Be("Updated Name");
            updatedProduct.Description.Should().Be("Updated Description");
            updatedProduct.Price.Should().Be(150);
            updatedProduct.Stock.Should().Be(30);
        }

        [Fact]
        public async Task DeleteProduct_ShouldRemoveProduct()
        {
            // Arrange
            var context = TestDbContextFactory.CreateDbContext();
            var service = new ProductService(context);
            var product = new Product { Name = "Product to Delete", Stock = 10 };
            var createdProduct = await service.CreateProductAsync(product);

            // Act
            var result = await service.DeleteProductAsync(createdProduct.ProductId);

            // Assert
            result.Should().BeTrue();
            var deletedProduct = await service.GetProductByIdAsync(createdProduct.ProductId);
            deletedProduct.Should().BeNull();
        }

        [Fact]
        public async Task DecrementStock_ShouldReduceStock()
        {
            // Arrange
            var context = TestDbContextFactory.CreateDbContext();
            var service = new ProductService(context);
            var product = new Product { Name = "Test Stock", Stock = 50 };
            var createdProduct = await service.CreateProductAsync(product);

            // Act
            var result = await service.DecrementStockAsync(createdProduct.ProductId, 20);

            // Assert
            result.Should().BeTrue();
            var updatedProduct = await service.GetProductByIdAsync(createdProduct.ProductId);
            updatedProduct.Stock.Should().Be(30);
        }

        [Fact]
        public async Task DecrementStock_ShouldFail_IfNotEnoughStock()
        {
            // Arrange
            var context = TestDbContextFactory.CreateDbContext();
            var service = new ProductService(context);
            var product = new Product 
            { 
                Name = "Test Stock",
                Description = "Test for decrement failure",
                Price = 100,
                Stock = 5 
            };
            var createdProduct = await service.CreateProductAsync(product);

            // Act
            var result = await service.DecrementStockAsync(createdProduct.ProductId, 10);

            // Assert
            result.Should().BeFalse();
            var updatedProduct = await service.GetProductByIdAsync(createdProduct.ProductId);
            updatedProduct.Stock.Should().Be(5); // Stock remains unchanged
        }

        [Fact]
        public async Task IncrementStock_ShouldIncreaseStock()
        {
            // Arrange
            var context = TestDbContextFactory.CreateDbContext();
            var service = new ProductService(context);
            var product = new Product 
            { 
                Name = "Test Stock Increase",
                Description = "Product used for increment stock test",
                Price = 200,
                Stock = 30 
            };
            var createdProduct = await service.CreateProductAsync(product);

            // Act
            var result = await service.IncrementStockAsync(createdProduct.ProductId, 10);

            // Assert
            result.Should().BeTrue();
            var updatedProduct = await service.GetProductByIdAsync(createdProduct.ProductId);
            updatedProduct.Stock.Should().Be(40);
        }
    }
}