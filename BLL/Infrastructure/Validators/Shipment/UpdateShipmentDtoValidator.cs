using BLL.DTO.Shipment;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Shipment
{
    public class UpdateShipmentDtoValidator : AbstractValidator<UpdateShipmentDto>
    {
        public UpdateShipmentDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.DeliveryId).GreaterThan(0);
            RuleFor(x => x.PaymentWayId).GreaterThan(0);
        }
    }
}
