using BLL.DTO.Book;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Book
{
    public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotNull().Length(1, 200);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price is less than zero.");
            RuleFor(x => x.IdPublisher).GreaterThan(0);
        }
    }
}
