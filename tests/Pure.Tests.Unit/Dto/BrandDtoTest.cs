using System;
using Pure.Application.Dto;
using Pure.Domain.Models;
using Xunit;

namespace Pure.Tests.Unit.Dto
{
    public class BrandDtoTest
    {
        private readonly Brand _brand;

        public BrandDtoTest()
        {
            _brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Acme"
            };
        }
        
        [Fact]
        public void Create_New_BrandDto()
        {
            // Act
            var result = new BrandDto();

            // Assert
            Assert.Equal(Guid.Empty, result.Id);
        }
        
        [Fact]
        public void Create_From_Brand_Entity()
        {
            // Act
            var result = new BrandDto(_brand);

            // Assert
            Assert.Equal(_brand.Id, result.Id);
            Assert.Equal(_brand.Name, result.Name);
        }
        
        [Fact]
        public void Convert_To_Brand_Entity()
        {
            // Arrange
            var productDto = new BrandDto(_brand);
            
            // Act
            var result = (Brand) productDto;

            // Assert
            Assert.Equal(_brand.Id, result.Id);
            Assert.Equal(_brand.Name, result.Name);
        }
    }
}