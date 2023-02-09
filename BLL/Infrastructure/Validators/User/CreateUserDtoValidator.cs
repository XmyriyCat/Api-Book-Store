using BLL.DTO.User;
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
            RuleFor(x => x.Email).EmailAddress().Length(0,150);
            RuleFor(x => x.Country).Length(0, 150);
            RuleFor(x => x.City).Length(0, 150);
            RuleFor(x => x.Address).Length(0, 150);
            RuleForEach(x => x.RolesIds).GreaterThan(0);
        }
    }
}
