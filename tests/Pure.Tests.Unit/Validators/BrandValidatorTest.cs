using System;
using FluentValidation.TestHelper;
using Pure.Domain.Models;
using Pure.Domain.Validators;
using Xunit;

namespace Pure.Tests.Unit.Validators
{
    public class BrandValidatorTest
    {
        private readonly BrandValidator _validator;
        private readonly Brand _brand;

        public BrandValidatorTest()
        {
            _validator = new BrandValidator();
            _brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Acme"
            };
        }
        
        [Fact]
        public void IsValid_Valid_Model()
        {
            // Act
            var result = _validator.TestValidate(_brand);

            // Assert
            Assert.NotNull(result);
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }
        
        [Fact]
        public void IsValid_Invalid_Id()
        {
            // Arrange
            _brand.Id = Guid.Empty;

            // Act
            var result = _validator.TestValidate(_brand);

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
            _brand.Name = productName;

            // Act
            var result = _validator.TestValidate(_brand);

            // Assert
            Assert.NotNull(result);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}