using BLL.DTO.Warehouse;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Warehouse
{
    public class CreateWarehouseDtoValidator : AbstractValidator<CreateWarehouseDto>
    {
        public CreateWarehouseDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(1,150);
            RuleFor(x => x.Country).NotNull().Length(1, 150);
            RuleFor(x => x.City).NotNull().Length(1, 150);
            RuleFor(x => x.Address).NotNull().Length(1, 150);
            RuleFor(x => x.PhoneNumber).NotNull().Length(1, 150);
        }
    }
}
