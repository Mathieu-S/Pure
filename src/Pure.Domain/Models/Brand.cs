using System;
using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;

namespace Pure.Domain.Models
{
    public record Brand
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }

        public void UpdateBrand(Brand brand)
        {
            Guard.Against.Null(brand, nameof(brand));

            Name = brand.Name;
        }
    }
}