using FluentValidation;
using Pure.Domain.Models;

namespace Pure.Domain.Validators
{
    public class BrandValidator : AbstractValidator<Brand>
    {
        public BrandValidator()
        {
            RuleFor(brand => brand.Name).NotEmpty();
        }
    }
}