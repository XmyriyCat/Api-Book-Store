using BLL.DTO.User;
using FluentValidation;

namespace BLL.Infrastructure.Validators.User
{
    public class LoginGoogleUserDtoValidator : AbstractValidator<LoginGoogleUserDto>
    {
        public LoginGoogleUserDtoValidator()
        {
            RuleFor(x => x.GoogleToken).NotNull().NotEmpty();
        }
    }
}
