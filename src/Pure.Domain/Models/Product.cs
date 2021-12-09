using System;
using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;

namespace Pure.Domain.Models
{
    public record Product
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public Brand Brand { get; set; }
        public uint Price { get; set; }

        public void UpdateProduct(Product product)
        {
            Guard.Against.Null(product, nameof(product));

            Name = product.Name;
            Description = product.Description;
            Brand = product.Brand;
            Price = product.Price;
        }
    }
}