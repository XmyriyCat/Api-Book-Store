using BLL.DTO.PaymentWay;
using FluentValidation;

namespace BLL.Infrastructure.Validators.PaymentWay
{
    public class UpdatePaymentWayValidator : AbstractValidator<UpdatePaymentWayDto>
    {
        public UpdatePaymentWayValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotNull().Length(1, 150);
        }
    }
}
