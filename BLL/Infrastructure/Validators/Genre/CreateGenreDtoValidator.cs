using BLL.DTO.Genre;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Genre
{
    public class CreateGenreDtoValidator : AbstractValidator<CreateGenreDto>
    {
        public CreateGenreDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(1, 150);
        }
    }
}
