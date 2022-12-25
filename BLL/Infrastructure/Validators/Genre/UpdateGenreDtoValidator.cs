using System.Data;
using BLL.DTO.Genre;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Genre
{
    public class UpdateGenreDtoValidator : AbstractValidator<UpdateGenreDto>
    {
        public UpdateGenreDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotNull().Length(1, 150);
        }
    }
}
