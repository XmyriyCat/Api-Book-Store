using BLL.DTO.User;
using FluentValidation;

namespace BLL.Infrastructure.Validators.User
{
    public class RegistrationUserDtoValidator : AbstractValidator<RegistrationUserDto>
    {
        public RegistrationUserDtoValidator()
        {
            RuleFor(x => x.Username).NotNull().Length(0, 150);
            RuleFor(x => x.Login).NotNull().Length(1, 150);
            RuleFor(x => x.Password).NotNull().Length(1, 150);
        }
    }
}
