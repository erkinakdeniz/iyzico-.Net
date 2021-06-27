
using Business.Handlers.Products.Commands;
using FluentValidation;

namespace Business.Handlers.Products.ValidationRules
{

    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.SubCategoryId).NotEmpty();
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.UnitsInStock).NotEmpty();
            RuleFor(x => x.UnitPrice).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.BrandId).NotEmpty();

        }
    }
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.SubCategoryId).NotEmpty();
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.UnitsInStock).NotEmpty();
            RuleFor(x => x.UnitPrice).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.BrandId).NotEmpty();

        }
    }
}