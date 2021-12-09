using System;
using Ardalis.GuardClauses;
using Pure.Domain.Models;

namespace Pure.Application.Dto
{
    /// <summary>
    /// An public representation of <see cref="Product"/> entity.
    /// </summary>
    public record ProductDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Brand { get; init; }
        public uint Price { get; init; }

        /// <summary>
        /// Ctor
        /// </summary>
        public ProductDto()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductDto(Product product)
        {
            Guard.Against.Null(product, nameof(product));

            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Brand = product.Brand?.Name;
            Price = product.Price;
        }

        /// <summary>
        /// An explicit operator to convert a <see cref="ProductDto"/> in <see cref="Product"/> entity.
        /// </summary>
        /// <param name="productDto">The dto</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static explicit operator Product(ProductDto productDto)
        {
            Guard.Against.Null(productDto, nameof(productDto));

            return new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price
            };
        }
    }
}