using System;
using Pure.Domain.Models;
using Xunit;

namespace Pure.Tests.Unit.Models
{
    public class ProductTest
    {
        private readonly Product _product;

        public ProductTest()
        {
            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Acme"
            };
            
            _product = new Product
            {
                Name = "Cola",
                Description = "A soda.",
                Brand = brand,
                Price = 1
            };
        }
        
        [Fact]
        public void UpdateProduct()
        {
            // Arrange
            var productWithNewData = new Product
            {
                Name = "Cola",
                Description = "A sweet drink.",
                Price = 2,
            };

            // Act
            _product.UpdateProduct(productWithNewData);

            // Assert
            Assert.Equal(_product.Name, productWithNewData.Name);
            Assert.Equal(_product.Description, productWithNewData.Description);
            Assert.Equal(_product.Price, productWithNewData.Price);
        }

        [Fact]
        public void UpdateProduct_Trow_Exception_With_Null_Arg()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _product.UpdateProduct(null));
        }
    }
}