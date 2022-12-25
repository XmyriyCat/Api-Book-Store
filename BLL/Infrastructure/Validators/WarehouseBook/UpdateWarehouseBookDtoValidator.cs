using BLL.DTO.WarehouseBook;
using FluentValidation;

namespace BLL.Infrastructure.Validators.WarehouseBook
{
    public class UpdateWarehouseBookDtoValidator : AbstractValidator<UpdateWarehouseBookDto>
    {
        public UpdateWarehouseBookDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.WarehouseId).GreaterThan(0);
            RuleFor(x => x.BookId).GreaterThan(0);
        }
    }
}
