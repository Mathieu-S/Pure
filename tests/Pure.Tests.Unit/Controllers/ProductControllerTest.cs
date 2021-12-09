using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Pure.Application.Commons;
using Pure.Application.Dto;
using Pure.Application.Exceptions;
using Pure.WebApi.Controllers;
using Xunit;

namespace Pure.Tests.Unit.Controllers
{
    public class ProductControllerTest
    {
        private readonly ProductController _controller;
        private readonly Mock<IProductHandler> _mockProductHandler;

        public ProductControllerTest()
        {
            var logger = new NullLogger<ProductController>();
            _mockProductHandler = new Mock<IProductHandler>();
            _controller = new ProductController(logger, _mockProductHandler.Object);
        }

        [Fact]
        public async Task Get_All()
        {
            // Arrange
            _mockProductHandler
                .Setup(handler => handler.GetAllProductsAsync())
                .ReturnsAsync(new List<ProductDto>());

            // Act
            var response = await _controller.Get();

            // Assert
            Assert.NotNull(response);
            var content = Assert.IsAssignableFrom<OkObjectResult>(response);
            Assert.NotNull(content.Value);
            Assert.IsAssignableFrom<IEnumerable<ProductDto>>(content.Value);
        }

        [Fact]
        public async Task Get_One()
        {
            // Arrange
            var id = new Guid("3df245fc-2f37-4282-a91a-97785e819be1");
            _mockProductHandler
                .Setup(handler => handler.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new ProductDto { Id = id });

            // Act
            var response = await _controller.Get(id);

            // Assert
            Assert.NotNull(response);
            var content = Assert.IsAssignableFrom<OkObjectResult>(response);
            Assert.NotNull(content.Value);
            var productDto = Assert.IsAssignableFrom<ProductDto>(content.Value);
            Assert.Equal(id, productDto.Id);
        }

        [Fact]
        public async Task Get_One_NotFount()
        {
            // Arrange
            _mockProductHandler
                .Setup(handler => handler.GetProductAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new EntityNotFoundException());

            // Act
            var response = await _controller.Get(Guid.NewGuid());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<NotFoundResult>(response);
        }

        [Fact]
        public async Task Get_One_BadRequest()
        {
            // Arrange
            _mockProductHandler
                .Setup(handler => handler.GetProductAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new ArgumentException());

            // Act
            var response = await _controller.Get(Guid.NewGuid());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task Post()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockProductHandler
                .Setup(handler => handler.CreateProductAsync(It.IsAny<ProductDto>()))
                .ReturnsAsync(productId);

            // Act
            var response = await _controller.Post(new ProductDto());

            // Assert
            Assert.NotNull(response);
            var content = Assert.IsAssignableFrom<CreatedAtActionResult>(response);
            Assert.NotNull(content.Value);
            Assert.Equal("Get", content.ActionName);
            Assert.Equal(productId.ToString(), content.Value);
        }

        [Fact]
        public async Task Post_BadRequest()
        {
            // Arrange
            _mockProductHandler
                .Setup(handler => handler.CreateProductAsync(It.IsAny<ProductDto>()))
                .ThrowsAsync(new ArgumentException());

            // Act
            var response = await _controller.Post(new ProductDto());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
        }
        
        [Fact]
        public async Task Post_ValidationException()
        {
            // Arrange
            _mockProductHandler
                .Setup(handler => handler.CreateProductAsync(It.IsAny<ProductDto>()))
                .ThrowsAsync(new ValidationException(""));

            // Act
            var response = await _controller.Post(new ProductDto());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task Put()
        {
            // Arrange
            var productToUpdate = new ProductDto { Id = new Guid("0ee23959-9c07-4f39-8305-e1cdc58bd9ca"), Price = 5 };
            _mockProductHandler
                .Setup(handler => handler.UpdateProductAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                .ReturnsAsync(productToUpdate with { Price = 6 });

            // Act
            var response = await _controller.Put(Guid.NewGuid(), productToUpdate);

            // Assert
            Assert.NotNull(response);
            var content = Assert.IsAssignableFrom<OkObjectResult>(response);
            Assert.NotNull(content.Value);
            var productDto = Assert.IsAssignableFrom<ProductDto>(content.Value);
            Assert.Equal(productToUpdate.Id, productDto.Id);
            Assert.NotEqual(productToUpdate.Price, productDto.Price);
        }
        
        [Fact]
        public async Task Put_NotFound()
        {
            // Arrange
            _mockProductHandler
                .Setup(handler => handler.UpdateProductAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                .ThrowsAsync(new EntityNotFoundException());

            // Act
            var response = await _controller.Put(Guid.NewGuid(), new ProductDto());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<NotFoundResult>(response);
        }
        
        [Fact]
        public async Task Put_BadRequest()
        {
            // Arrange
            _mockProductHandler
                .Setup(handler => handler.UpdateProductAsync(It.IsAny<Guid>(), It.IsAny<ProductDto>()))
                .ThrowsAsync(new ArgumentException());

            // Act
            var response = await _controller.Put(Guid.NewGuid(), new ProductDto());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
        }
        
        [Fact]
        public async Task Delete()
        {
            // Arrange
            _mockProductHandler
                .Setup(handler => handler.DeleteProductAsync(It.IsAny<Guid>()));

            // Act
            var response = await _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<NoContentResult>(response);
        }
        
        [Fact]
        public async Task Delete_NotFound()
        {
            // Arrange
            _mockProductHandler
                .Setup(handler => handler.DeleteProductAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new EntityNotFoundException());

            // Act
            var response = await _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<NotFoundResult>(response);
        }
        
        [Fact]
        public async Task Delete_BadRequest()
        {
            // Arrange
            _mockProductHandler
                .Setup(handler => handler.DeleteProductAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new ArgumentException());

            // Act
            var response = await _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
        }
    }
}