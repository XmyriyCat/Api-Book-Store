using BLL.DTO.User;
using FluentValidation;

namespace BLL.Infrastructure.Validators.User
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(x => x.Login).NotNull().Length(1, 150);
            RuleFor(x => x.Password).NotNull().Length(1, 150);
        }
    }
}
