using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Moq;
using Pure.Application.Commons;
using Pure.Application.Dto;
using Pure.Application.Exceptions;
using Pure.Application.Handlers;
using Pure.Application.Repositories;
using Pure.Domain.Models;
using Xunit;

namespace Pure.Tests.Unit.Handlers
{
    public class ProductHandlerTest
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IBrandRepository> _mockBrandRepository;
        private readonly IProductHandler _productHandler;
        private readonly IEnumerable<Product> _products;

        public ProductHandlerTest()
        {
            var mockProductValidator = new Mock<IValidator<Product>>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockBrandRepository = new Mock<IBrandRepository>();
            _productHandler = new ProductHandler(mockProductValidator.Object, _mockProductRepository.Object, _mockBrandRepository.Object);
            _products = new List<Product>
            {
                new() { Id = Guid.NewGuid(), Name = "Cola", Description = "A soda", Price = 1 },
                new() { Id = Guid.NewGuid(), Name = "Pizza", Description = "A neapolitan pizza", Price = 5 }
            };
        }

        [Fact]
        public async Task Get_All_Products()
        {
            // Arrange
            _mockProductRepository.Setup(x => x.GetAsync()).ReturnsAsync(_products);

            // Act
            var result = await _productHandler.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ProductDto>>(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Get_One_Product()
        {
            // Arrange
            _mockProductRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_products.FirstOrDefault());

            // Act
            var result = await _productHandler.GetProductAsync(Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ProductDto>(result);
        }

        [Fact]
        public async Task Get_One_Without_Valid_Guid()
        {
            // Arrange
            _mockProductRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_products.FirstOrDefault());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _productHandler.GetProductAsync(Guid.Empty));
        }

        [Fact]
        public async Task Get_One_Not_Found()
        {
            // Arrange
            _mockProductRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _productHandler.GetProductAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task Create_A_Product()
        {
            // Arrange
            var product = new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Hot dog",
                Description = "Not contain dog meat.",
                Price = 2
            };
            _mockProductRepository.Setup(x => x.AddAsync(It.IsAny<Product>())).ReturnsAsync(product.Id);

            // Act
            var result = await _productHandler.CreateProductAsync(product);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(product.Id, result);
        }

        [Fact]
        public async Task Create_A_Product_With_Null_Dto()
        {
            // Arrange
            _mockProductRepository.Setup(x => x.AddAsync(It.IsAny<Product>())).ReturnsAsync(Guid.Empty);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _productHandler.CreateProductAsync(null));
        }

        [Fact]
        public async Task Update_A_Product()
        {
            // Arrange
            var product = new ProductDto { Id = Guid.NewGuid() };
            _mockProductRepository.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(new Product());
            _mockProductRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>()));

            // Act & Assert
            await _productHandler.UpdateProductAsync(product.Id, product);
        }

        [Fact]
        public async Task Update_A_Product_Not_Found()
        {
            // Arrange
            var product = new ProductDto { Id = Guid.NewGuid() };
            _mockProductRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);
            _mockProductRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>()));

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _productHandler.UpdateProductAsync(product.Id, product));
        }

        [Fact]
        public async Task Delete_A_Product()
        {
            // Arrange
            _mockProductRepository.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(new Product());
            _mockProductRepository.Setup(x => x.DeleteAsync(It.IsAny<Product>()));

            // Act & Assert
            await _productHandler.DeleteProductAsync(Guid.NewGuid());
        }

        [Fact]
        public async Task Delete_A_Product_Not_Found()
        {
            // Arrange
            _mockProductRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _productHandler.DeleteProductAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task Delete_A_Product_With_Null_Dto()
        {
            // Arrange
            _mockProductRepository.Setup(x => x.DeleteAsync(It.IsAny<Product>()));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _productHandler.DeleteProductAsync(Guid.Empty));
        }
    }
}