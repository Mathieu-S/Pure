using FluentValidation;
using Pure.Domain.Models;

namespace Pure.Domain.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Name).NotEmpty();
            RuleFor(product => product.Price).GreaterThan((uint)0);
            RuleFor(product => product.Brand).SetValidator(new BrandValidator());
        }
    }
}