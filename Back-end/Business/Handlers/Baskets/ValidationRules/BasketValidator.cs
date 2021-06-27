
using Business.Handlers.Baskets.Commands;
using FluentValidation;

namespace Business.Handlers.Baskets.ValidationRules
{

    public class CreateBasketValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketValidator()
        {
            RuleFor(x => x.ProductID).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();

        }
    }
    public class UpdateBasketValidator : AbstractValidator<UpdateBasketCommand>
    {
        public UpdateBasketValidator()
        {
            RuleFor(x => x.ProductID).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();

        }
    }
}