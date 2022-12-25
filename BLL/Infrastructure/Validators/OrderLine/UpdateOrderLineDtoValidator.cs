using BLL.DTO.OrderLine;
using FluentValidation;

namespace BLL.Infrastructure.Validators.OrderLine
{
    public class UpdateOrderLineDtoValidator : AbstractValidator<UpdateOrderLineDto>
    {
        public UpdateOrderLineDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.WarehouseBookId).GreaterThan(0);
        }
    }
}
