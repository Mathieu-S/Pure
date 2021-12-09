using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FluentValidation;
using Pure.Application.Commons;
using Pure.Application.Dto;
using Pure.Application.Exceptions;
using Pure.Application.Extensions;
using Pure.Application.Repositories;
using Pure.Domain.Models;

namespace Pure.Application.Handlers
{
    /// <inheritdoc/>
    public class ProductHandler : IProductHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IValidator<Product> _productValidator;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="productValidator"></param>
        /// <param name="productRepository"></param>
        /// <param name="brandRepository"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductHandler(IValidator<Product> productValidator, IProductRepository productRepository, IBrandRepository brandRepository)
        {
            _productValidator = Guard.Against.Null(productValidator, nameof(productValidator));
            _productRepository = Guard.Against.Null(productRepository, nameof(productRepository));
            _brandRepository = Guard.Against.Null(brandRepository, nameof(brandRepository));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAsync();
            return products.Select(x => new ProductDto(x)).ToList();
        }

        /// <inheritdoc/>
        public async Task<ProductDto> GetProductAsync(Guid productId)
        {
            Guard.Against.NullOrEmpty(productId, nameof(productId));

            var product = await _productRepository.GetAsync(productId);

            if (product is null)
            {
                throw new EntityNotFoundException($"The product with ID:{productId} was not found.");
            }

            return new ProductDto(product);
        }

        /// <inheritdoc/>
        public async Task<Guid> CreateProductAsync(ProductDto productDto)
        {
            Guard.Against.Null(productDto, nameof(productDto));

            // Validate
            var product = (Product)productDto;
            await _productValidator.ValidateAndThrowAsync(product);
            
            // Check if Brand exist
            var brand = await _brandRepository.GetByName(productDto.Brand, true);
            product.Brand = brand ?? new Brand { Name = productDto.Brand };
            
            return await _productRepository.AddAsync(product);
        }

        /// <inheritdoc/>
        public async Task<ProductDto> UpdateProductAsync(Guid productId, ProductDto productDto)
        {
            Guard.Against.NullOrEmpty(productId, nameof(productId));
            Guard.Against.Null(productDto, nameof(productDto));
            Guard.Against.NotSameId(productId, productDto);

            // Check if product exist
            var productToUpdate = await _productRepository.GetAsync(productId, true);
            if (productToUpdate is null)
            {
                throw new EntityNotFoundException($"The product with ID:{productId} was not found.");
            }

            // Validate
            var product = (Product)productDto;
            await _productValidator.ValidateAndThrowAsync(product);
            
            // Check if Brand exist
            var brand = await _brandRepository.GetByName(productDto.Brand, true);
            if (brand is null)
            {
                throw new EntityNotFoundException("The brand does not exist.");
            }
            productToUpdate.Brand = brand;

            productToUpdate.UpdateProduct((Product)productDto);
            await _productRepository.UpdateAsync(productToUpdate);

            return productDto;
        }

        /// <inheritdoc/>
        public async Task DeleteProductAsync(Guid productId)
        {
            Guard.Against.NullOrEmpty(productId, nameof(productId));

            var productToDelete = await _productRepository.GetAsync(productId, true);

            if (productToDelete is null)
            {
                throw new EntityNotFoundException($"The product with ID:{productId} was not found.");
            }

            await _productRepository.DeleteAsync(productToDelete);
        }
    }
}