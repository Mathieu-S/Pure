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
    public class BrandControllerTest
    {
        private readonly BrandController _controller;
        private readonly Mock<IBrandHandler> _mockBrandHandler;
        
        public BrandControllerTest()
        {
            var logger = new NullLogger<BrandController>();
            _mockBrandHandler = new Mock<IBrandHandler>();
            _controller = new BrandController(logger, _mockBrandHandler.Object);
        }

        [Fact]
        public async Task Get_All()
        {
            // Arrange
            _mockBrandHandler
                .Setup(handler => handler.GetAllBrandsAsync())
                .ReturnsAsync(new List<BrandDto>());

            // Act
            var response = await _controller.Get();

            // Assert
            Assert.NotNull(response);
            var content = Assert.IsAssignableFrom<OkObjectResult>(response);
            Assert.NotNull(content.Value);
            Assert.IsAssignableFrom<IEnumerable<BrandDto>>(content.Value);
        }
        
        [Fact]
        public async Task Get_One()
        {
            // Arrange
            var id = new Guid("2c9f6032-9d43-4fa2-ac64-9c5750f0c504");
            _mockBrandHandler
                .Setup(handler => handler.GetBrandAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new BrandDto { Id = id });

            // Act
            var response = await _controller.Get(id);

            // Assert
            Assert.NotNull(response);
            var content = Assert.IsAssignableFrom<OkObjectResult>(response);
            Assert.NotNull(content.Value);
            var brandDto = Assert.IsAssignableFrom<BrandDto>(content.Value);
            Assert.Equal(id, brandDto.Id);
        }
        
        [Fact]
        public async Task Get_One_NotFount()
        {
            // Arrange
            _mockBrandHandler
                .Setup(handler => handler.GetBrandAsync(It.IsAny<Guid>()))
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
            _mockBrandHandler
                .Setup(handler => handler.GetBrandAsync(It.IsAny<Guid>()))
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
            var brandId = Guid.NewGuid();
            _mockBrandHandler
                .Setup(handler => handler.CreateBrandAsync(It.IsAny<BrandDto>()))
                .ReturnsAsync(brandId);

            // Act
            var response = await _controller.Post(new BrandDto());

            // Assert
            Assert.NotNull(response);
            var content = Assert.IsAssignableFrom<CreatedAtActionResult>(response);
            Assert.NotNull(content.Value);
            Assert.Equal("Get", content.ActionName);
            Assert.Equal(brandId.ToString(), content.Value);
        }
        
        [Fact]
        public async Task Post_BadRequest()
        {
            // Arrange
            _mockBrandHandler
                .Setup(handler => handler.CreateBrandAsync(It.IsAny<BrandDto>()))
                .ThrowsAsync(new ArgumentException());

            // Act
            var response = await _controller.Post(new BrandDto());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
        }
        
        [Fact]
        public async Task Post_ValidationException()
        {
            // Arrange
            _mockBrandHandler
                .Setup(handler => handler.CreateBrandAsync(It.IsAny<BrandDto>()))
                .ThrowsAsync(new ValidationException(""));

            // Act
            var response = await _controller.Post(new BrandDto());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
        }
        
        [Fact]
        public async Task Put()
        {
            // Arrange
            var brandToUpdate = new BrandDto { Id = new Guid("2c9f6032-9d43-4fa2-ac64-9c5750f0c504"), Name = "Acme" };
            _mockBrandHandler
                .Setup(handler => handler.UpdateBrandAsync(It.IsAny<Guid>(), It.IsAny<BrandDto>()))
                .ReturnsAsync(brandToUpdate with { Name = "Aperture" });

            // Act
            var response = await _controller.Put(Guid.NewGuid(), brandToUpdate);

            // Assert
            Assert.NotNull(response);
            var content = Assert.IsAssignableFrom<OkObjectResult>(response);
            Assert.NotNull(content.Value);
            var brandDto = Assert.IsAssignableFrom<BrandDto>(content.Value);
            Assert.Equal(brandToUpdate.Id, brandDto.Id);
            Assert.NotEqual(brandToUpdate.Name, brandDto.Name);
        }
        
        [Fact]
        public async Task Put_NotFound()
        {
            // Arrange
            _mockBrandHandler
                .Setup(handler => handler.UpdateBrandAsync(It.IsAny<Guid>(), It.IsAny<BrandDto>()))
                .ThrowsAsync(new EntityNotFoundException());

            // Act
            var response = await _controller.Put(Guid.NewGuid(), new BrandDto());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<NotFoundResult>(response);
        }
        
        [Fact]
        public async Task Put_BadRequest()
        {
            // Arrange
            _mockBrandHandler
                .Setup(handler => handler.UpdateBrandAsync(It.IsAny<Guid>(), It.IsAny<BrandDto>()))
                .ThrowsAsync(new ArgumentException());

            // Act
            var response = await _controller.Put(Guid.NewGuid(), new BrandDto());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
        }
        
        [Fact]
        public async Task Delete()
        {
            // Arrange
            _mockBrandHandler
                .Setup(handler => handler.DeleteBrandAsync(It.IsAny<Guid>()));

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
            _mockBrandHandler
                .Setup(handler => handler.DeleteBrandAsync(It.IsAny<Guid>()))
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
            _mockBrandHandler
                .Setup(handler => handler.DeleteBrandAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new ArgumentException());

            // Act
            var response = await _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<BadRequestObjectResult>(response);
        }
    }
}