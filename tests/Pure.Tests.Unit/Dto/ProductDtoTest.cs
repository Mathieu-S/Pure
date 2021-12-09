using System;
using Pure.Application.Dto;
using Pure.Domain.Models;
using Xunit;

namespace Pure.Tests.Unit.Dto
{
    public class ProductDtoTest
    {
        private readonly Product _product;

        public ProductDtoTest()
        {
            _product = new Product
            {
                Name = "Cola",
                Description = "A soda.",
                Price = 1
            };
        }
        
        [Fact]
        public void Create_New_ProductDto()
        {
            // Act
            var result = new ProductDto();

            // Assert
            Assert.Equal(Guid.Empty, result.Id);
        }
        
        [Fact]
        public void Create_From_Product_Entity()
        {
            // Act
            var result = new ProductDto(_product);

            // Assert
            Assert.Equal(_product.Id, result.Id);
            Assert.Equal(_product.Name, result.Name);
            Assert.Equal(_product.Description, result.Description);
            Assert.Equal(_product.Price, result.Price);
        }

        [Fact]
        public void Convert_To_Product_Entity()
        {
            // Arrange
            var productDto = new ProductDto(_product);
            
            // Act
            var result = (Product) productDto;

            // Assert
            Assert.Equal(_product.Id, result.Id);
            Assert.Equal(_product.Name, result.Name);
            Assert.Equal(_product.Description, result.Description);
            Assert.Equal(_product.Price, result.Price);
        }
    }
}