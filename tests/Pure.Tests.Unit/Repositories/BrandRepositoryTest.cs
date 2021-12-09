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
    public class BrandRepositoryTest
    {
        private readonly BrandRepository _brandRepository;

        public BrandRepositoryTest(DatabaseFixture fixture)
        {
            _brandRepository = new BrandRepository(fixture.PureDbContext);
        }

        [Fact]
        public async Task Get_All_Brands()
        {
            // Act
            var result = await _brandRepository.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IList<Brand>>(result);
            Assert.NotEmpty(result);
        }
        
        [Fact]
        public async Task Get_A_Brand()
        {
            // Arrange
            var brandId = new Guid("2c9f6032-9d43-4fa2-ac64-9c5750f0c504");
            
            // Act
            var result = await _brandRepository.GetAsync(brandId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Brand>(result);
            Assert.Equal(brandId, result.Id);
        }
        
        [Fact]
        public async Task Get_Unknown_Brand()
        {
            // Act
            var result = await _brandRepository.GetAsync(Guid.Empty);

            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task Find_A_Brand_By_Name()
        {
            // Arrange
            const string brandName = "Acme";
            
            // Act
            var result = await _brandRepository.GetByName(brandName);

            // Assert
            Assert.Null(result);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Apple")]
        public async Task Find_A_Brand_By_Name_NotFound(string brandName)
        {
            // Act
            var result = await _brandRepository.GetByName(brandName);

            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task Add_A_Product()
        {
            // Arrange
            var brand = new Brand
            {
                Id = Guid.Empty,
                Name = "Aperture"
            };

            // Act
            var result = await _brandRepository.AddAsync(brand);

            // Assert
            var brandsInDatabase = await _brandRepository.GetAsync();
            Assert.NotEqual(Guid.Empty, result);
            Assert.Contains(brand, brandsInDatabase);
        }
        
        [Fact]
        public async Task Update_A_Product()
        {
            // Arrange
            const string newBrandName = "Black Mesa";
            var brand = await _brandRepository.GetAsync(new Guid("2c9f6032-9d43-4fa2-ac64-9c5750f0c504"), true);

            // Act
            brand.Name = newBrandName;
            await _brandRepository.UpdateAsync(brand);

            // Assert
            var modifiedBrand = await _brandRepository.GetAsync(brand.Id);
            Assert.Equal(newBrandName, modifiedBrand.Name);
        }
        
        [Fact]
        public async Task Delete_A_Product()
        {
            // Arrange
            var brand = await _brandRepository.GetAsync(new Guid("2c9f6032-9d43-4fa2-ac64-9c5750f0c504"), true);

            // Act
            await _brandRepository.DeleteAsync(brand);

            // Assert
            var brandsInDatabase = await _brandRepository.GetAsync();
            Assert.DoesNotContain(brand, brandsInDatabase);
        }
    }
}