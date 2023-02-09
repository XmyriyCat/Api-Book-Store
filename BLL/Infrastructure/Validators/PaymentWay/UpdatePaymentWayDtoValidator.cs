using BLL.DTO.PaymentWay;
using FluentValidation;

namespace BLL.Infrastructure.Validators.PaymentWay
{
    public class UpdatePaymentWayDtoValidator : AbstractValidator<UpdatePaymentWayDto>
    {
        public UpdatePaymentWayDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotNull().Length(1, 150);
        }
    }
}
