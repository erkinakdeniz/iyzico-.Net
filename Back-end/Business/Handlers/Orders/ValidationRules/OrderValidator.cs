
using Business.Handlers.Orders.Commands;
using FluentValidation;

namespace Business.Handlers.Orders.ValidationRules
{

    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.OrderNumber).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
            RuleFor(x => x.Massage).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.OrderDate).NotEmpty();
            RuleFor(x => x.ShipCity).NotEmpty();

        }
    }
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.OrderNumber).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
            RuleFor(x => x.Massage).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.OrderDate).NotEmpty();
            RuleFor(x => x.ShipCity).NotEmpty();

        }
    }
}