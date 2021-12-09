using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Pure.Application.Commons;
using Pure.Application.Dto;
using Pure.Application.Exceptions;
using Pure.Application.Extensions;
using Pure.Application.Repositories;
using Pure.Domain.Models;

namespace Pure.Application.Handlers
{
    /// <inheritdoc />
    public class BrandHandler : IBrandHandler
    {
        private readonly IBrandRepository _brandRepository;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="brandRepository"></param>
        public BrandHandler(IBrandRepository brandRepository)
        {
            _brandRepository = Guard.Against.Null(brandRepository, nameof(brandRepository));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.GetAsync();
            return brands.Select(x => new BrandDto(x)).ToList();
        }

        /// <inheritdoc />
        public async Task<BrandDto> GetBrandAsync(Guid brandId)
        {
            Guard.Against.NullOrEmpty(brandId, nameof(brandId));

            var brand = await _brandRepository.GetAsync(brandId);

            if (brand is null)
            {
                throw new EntityNotFoundException($"The brand with ID:{brandId} was not found.");
            }

            return new BrandDto(brand);
        }

        /// <inheritdoc />
        public async Task<Guid> CreateBrandAsync(BrandDto brandDto)
        {
            Guard.Against.Null(brandDto, nameof(brandDto));

            var brand = (Brand)brandDto;
            // await _productValidator.ValidateAndThrowAsync(product);

            return await _brandRepository.AddAsync(brand);
        }

        /// <inheritdoc />
        public async Task<BrandDto> UpdateBrandAsync(Guid brandId, BrandDto brandDto)
        {
            Guard.Against.NullOrEmpty(brandId, nameof(brandId));
            Guard.Against.Null(brandDto, nameof(brandDto));
            Guard.Against.NotSameId(brandId, brandDto);

            var brandToUpdate = await _brandRepository.GetAsync(brandId, true);

            if (brandToUpdate is null)
            {
                throw new EntityNotFoundException($"The brand with ID:{brandId} was not found.");
            }

            brandToUpdate.UpdateBrand((Brand)brandDto);
            await _brandRepository.UpdateAsync(brandToUpdate);

            return brandDto;
        }

        /// <inheritdoc />
        public async Task DeleteBrandAsync(Guid brandId)
        {
            Guard.Against.NullOrEmpty(brandId, nameof(brandId));

            var brandToDelete = await _brandRepository.GetAsync(brandId, true);

            if (brandToDelete is null)
            {
                throw new EntityNotFoundException($"The brand with ID:{brandId} was not found.");
            }

            await _brandRepository.DeleteAsync(brandToDelete);
        }
    }
}