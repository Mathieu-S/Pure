using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pure.Application.Dto;
using Pure.Application.Exceptions;
using Pure.Domain.Models;

namespace Pure.Application.Commons
{
    /// <summary>
    /// An handler for requests on the <see cref="Brand"/> repository.
    /// </summary>
    public interface IBrandHandler
    {
        /// <summary>
        /// Get all brands from brand repository.
        /// </summary>
        /// <returns>A list of converted <see cref="Brand"/> into <see cref="BrandDto"/>.</returns>
        public Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
        
        /// <summary>
        /// Get a specified brand from brand repository.
        /// </summary>
        /// <param name="brandId">The brand Guid.</param>
        /// <returns>A <see cref="Product"/> converted into a <see cref="BrandDto"/>.</returns>
        /// <exception cref="ArgumentException">Throw when <see cref="Guid"/> is empty.</exception>
        /// <exception cref="EntityNotFoundException">Throw when no <see cref="Brand"/> is found.</exception>
        public Task<BrandDto> GetBrandAsync(Guid brandId);
        
        /// <summary>
        /// Add a brand to the repository.
        /// </summary>
        /// <param name="brandDto">The <see cref="BrandDto"/> to add.</param>
        /// <returns>The Guid of created <see cref="Brand"/>.</returns>
        /// <exception cref="ArgumentNullException">Throw when the given <see cref="BrandDto"/> is null.</exception>
        public Task<Guid> CreateBrandAsync(BrandDto brandDto);
        
        /// <summary>
        /// Update the specified brand from brand repository.
        /// </summary>
        /// <param name="brandId">The brand Guid.</param>
        /// <param name="brandDto">The <see cref="BrandDto"/> to update.</param>
        /// <returns>The modified brand.</returns>
        /// <exception cref="ArgumentException">Throw when <see cref="Guid"/> is empty.</exception>
        /// <exception cref="ArgumentNullException">Throw when the given <see cref="BrandDto"/> is null.</exception>
        /// <exception cref="EntityNotFoundException">Throw when no <see cref="Brand"/> is found.</exception>
        public Task<BrandDto> UpdateBrandAsync(Guid brandId, BrandDto brandDto);
        
        /// <summary>
        /// Delete the specified brand from brand repository.
        /// </summary>
        /// <param name="brandId">The brand Guid.</param>
        /// <exception cref="ArgumentException">Throw when <see cref="Guid"/> is empty.</exception>
        /// <exception cref="EntityNotFoundException">Throw when no <see cref="Brand"/> is found.</exception>
        public Task DeleteBrandAsync(Guid brandId);
    }
}