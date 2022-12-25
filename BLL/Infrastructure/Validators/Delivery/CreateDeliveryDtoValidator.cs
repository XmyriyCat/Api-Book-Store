using BLL.DTO.Delivery;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Delivery
{
    public class CreateDeliveryDtoValidator : AbstractValidator<CreateDeliveryDto>
    {
        public CreateDeliveryDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(1, 150);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price is less than zero.");
        }
    }
}
