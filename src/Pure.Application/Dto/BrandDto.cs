using System;
using Ardalis.GuardClauses;
using Pure.Domain.Models;

namespace Pure.Application.Dto
{
    /// <summary>
    /// An public representation of <see cref="Brand"/> entity.
    /// </summary>
    public record BrandDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

        /// <summary>
        /// Ctor
        /// </summary>
        public BrandDto()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="brand"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BrandDto(Brand brand)
        {
            Guard.Against.Null(brand, nameof(brand));

            Id = brand.Id;
            Name = brand.Name;
        }

        /// <summary>
        /// An explicit operator to convert a <see cref="BrandDto"/> in <see cref="Brand"/> entity.
        /// </summary>
        /// <param name="brandDto">The dto</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static explicit operator Brand(BrandDto brandDto)
        {
            Guard.Against.Null(brandDto, nameof(brandDto));

            return new Brand
            {
                Id = brandDto.Id,
                Name = brandDto.Name
            };
        }
    }
}