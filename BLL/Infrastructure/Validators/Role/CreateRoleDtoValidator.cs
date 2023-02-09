using BLL.DTO.Role;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Role
{
    public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(1, 150);
        }
    }
}
