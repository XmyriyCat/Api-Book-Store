using BLL.DTO.OrderLine;
using FluentValidation;

namespace BLL.Infrastructure.Validators.OrderLine
{
    public class CreateOrderLineDtoValidator : AbstractValidator<CreateOrderLineDto>
    {
        public CreateOrderLineDtoValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.WarehouseBookId).GreaterThan(0);
        }
    }
}
