using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pure.Domain.Models;
using Pure.Persistence.Repositories;
using Pure.Tests.Unit.TestHelpers;
using Xunit;

namespace Pure.Tests.Unit.Repositories
{
    [Collection("Database collection")]
    public class ProductRepositoryTest
    {
        private readonly ProductRepository _productRepository;

        public ProductRepositoryTest(DatabaseFixture fixture)
        {
            _productRepository = new ProductRepository(fixture.PureDbContext);
        }

        [Fact]
        public async Task Get_All_Product()
        {
            // Act
            var result = await _productRepository.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IList<Product>>(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Get_A_Product()
        {
            // Arrange
            var productId = new Guid("fa023462-8ecf-4d63-97d6-ebece647299f");
            
            // Act
            var result = await _productRepository.GetAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Product>(result);
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async Task Get_Unknown_Product()
        {
            // Act
            var result = await _productRepository.GetAsync(Guid.Empty);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Add_A_Product()
        {
            // Arrange
            var product = new Product
            {
                Id = Guid.Empty,
                Name = "Hot dog",
                Description = "Not contain dog meat.",
                Price = 2
            };

            // Act
            var result = await _productRepository.AddAsync(product);

            // Assert
            var productsInDatabase = await _productRepository.GetAsync();
            Assert.NotEqual(Guid.Empty, result);
            Assert.Contains(product, productsInDatabase);
        }

        [Fact]
        public async Task Update_A_Product()
        {
            // Arrange
            var product = await _productRepository.GetAsync(new Guid("e0fe5fa6-374d-4eca-8534-5fc3e3d01f7c"), true);

            // Act
            product.Price = 4;
            await _productRepository.UpdateAsync(product);

            // Assert
            var modifiedProduct = await _productRepository.GetAsync(product.Id);
            Assert.Equal((uint)4, modifiedProduct.Price);
        }

        [Fact]
        public async Task Delete_A_Product()
        {
            // Arrange
            var product = await _productRepository.GetAsync(new Guid("fa023462-8ecf-4d63-97d6-ebece647299f"), true);

            // Act
            await _productRepository.DeleteAsync(product);

            // Assert
            var productsInDatabase = await _productRepository.GetAsync();
            Assert.DoesNotContain(product, productsInDatabase);
        }
    }
}