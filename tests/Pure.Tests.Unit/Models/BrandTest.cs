using System;
using Pure.Domain.Models;
using Xunit;

namespace Pure.Tests.Unit.Models
{
    public class BrandTest
    {
        private readonly Brand _brand;

        public BrandTest()
        {
            _brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Acme"
            };
        }
        
        [Fact]
        public void UpdateProduct()
        {
            // Arrange
            var brandWithNewData = new Brand()
            {
                Name = "Aperture"
            };

            // Act
            _brand.UpdateBrand(brandWithNewData);

            // Assert
            Assert.Equal(_brand.Name, brandWithNewData.Name);
        }

        [Fact]
        public void UpdateBrand_Trow_Exception_With_Null_Arg()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _brand.UpdateBrand(null));
        }
    }
}