using BLL.DTO.Delivery;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Delivery
{
    public class UpdateDeliveryDtoValidator : AbstractValidator<UpdateDeliveryDto>
    {
        public UpdateDeliveryDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotNull().Length(1, 150);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price is less than zero.");
        }
    }
}
