﻿using BLL.DTO.Order;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Order
{
    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.TotalPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.OrderDate).NotNull().NotEmpty();
            RuleFor(x => x.ShipmentId).GreaterThan(0);
            RuleFor(x => x.CustomerId).GreaterThan(0);
        }
    }
}
