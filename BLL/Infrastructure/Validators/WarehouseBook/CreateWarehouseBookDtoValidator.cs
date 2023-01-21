using BLL.DTO.WarehouseBook;
using FluentValidation;

namespace BLL.Infrastructure.Validators.WarehouseBook
{
    public class CreateWarehouseBookDtoValidator : AbstractValidator<CreateWarehouseBookDto>
    {
        public CreateWarehouseBookDtoValidator()
        {
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.WarehouseId).GreaterThan(0);
            RuleFor(x => x.BookId).GreaterThan(0);
        }
    }
}
