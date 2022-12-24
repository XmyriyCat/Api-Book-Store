using BLL.DTO.PaymentWay;
using FluentValidation;

namespace BLL.Infrastructure.Validators.PaymentWay
{
    public class CreatePaymentWayDtoValidator : AbstractValidator<CreatePaymentWayDto>
    {
        public CreatePaymentWayDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(1,150);
        }
    }
}
