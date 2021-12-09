using System;
using FluentValidation.TestHelper;
using Pure.Domain.Models;
using Pure.Domain.Validators;
using Xunit;

namespace Pure.Tests.Unit.Validators
{
    public class ProductValidatorTest
    {
        private readonly ProductValidator _validator;
        private readonly Product _product;

        public ProductValidatorTest()
        {
            _validator = new ProductValidator();
            _product = new Product
            {
                Id = new Guid("e0fe5fa6-374d-4eca-8534-5fc3e3d01f7c"),
                Name = "Pizza",
                Description = "A neapolitan pizza",
                Price = 5
            };
        }

        [Fact]
        public void IsValid_Valid_Model()
        {
            // Act
            var result = _validator.TestValidate(_product);

            // Assert
            Assert.NotNull(result);
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Brand);
        }
        
        [Fact]
        public void IsValid_Invalid_Id()
        {
            // Arrange
            _product.Id = Guid.Empty;

            // Act
            var result = _validator.TestValidate(_product);

            // Assert
            Assert.NotNull(result);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void IsValid_Invalid_Name(string productName)
        {
            // Arrange
            _product.Name = productName;

            // Act
            var result = _validator.TestValidate(_product);

            // Assert
            Assert.NotNull(result);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        
        [Fact]
        public void IsValid_Invalid_Price()
        {
            // Arrange
            _product.Price = 0;

            // Act
            var result = _validator.TestValidate(_product);

            // Assert
            Assert.NotNull(result);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }
    }
}