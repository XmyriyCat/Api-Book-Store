using BLL.DTO.Author;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Author
{
    public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
    {
        public UpdateAuthorDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.FirstName).NotNull().Length(1, 150);
            RuleFor(x => x.LastName).NotNull().Length(1, 150);
        }
    }
}
