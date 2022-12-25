using BLL.DTO.Author;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Author
{
    public class CreateAuthorDtoValidator : AbstractValidator<CreateAuthorDto>
    {
        public CreateAuthorDtoValidator()
        {
            RuleFor(x => x.FirstName).NotNull().Length(1, 150);
            RuleFor(x => x.LastName).NotNull().Length(1, 150);
        }
    }
}
