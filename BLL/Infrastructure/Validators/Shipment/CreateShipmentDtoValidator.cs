using BLL.DTO.Shipment;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Shipment
{
    public class CreateShipmentDtoValidator : AbstractValidator<CreateShipmentDto>
    {
        public CreateShipmentDtoValidator()
        {
            RuleFor(x => x.DeliveryId).GreaterThan(0);
            RuleFor(x => x.PaymentWayId).GreaterThan(0);
        }
    }
}
