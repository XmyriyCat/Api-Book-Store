using BLL.DTO.User;
using FluentValidation;

namespace BLL.Infrastructure.Validators.User
{
    public class RegistrationGoogleUserDtoValidator : AbstractValidator<RegistrationGoogleUserDto>
    {
        public RegistrationGoogleUserDtoValidator()
        {
            RuleFor(x => x.GoogleToken).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
