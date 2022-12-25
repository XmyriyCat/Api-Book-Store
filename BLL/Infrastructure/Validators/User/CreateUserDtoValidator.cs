using BLL.DTO.Users;
using FluentValidation;

namespace BLL.Infrastructure.Validators.User
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Username).NotNull().Length(0, 150);
            RuleFor(x => x.Login).NotNull().Length(1, 150);
            RuleFor(x => x.Password).NotNull().Length(1, 150);
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.Country).NotNull().Length(1, 150);
            RuleFor(x => x.City).NotNull().Length(1, 150);
            RuleFor(x => x.Address).NotNull().Length(1, 150);
            RuleFor(x => x.RolesIds).NotNull().NotEmpty();
            RuleForEach(x => x.RolesIds).GreaterThan(0);
        }
    }
}
