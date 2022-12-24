using BLL.DTO.Publisher;
using FluentValidation;

namespace BLL.Infrastructure.Validators.Publisher
{
    public class UpdatePublisherDtoValidator : AbstractValidator<UpdatePublisherDto>
    {
        public UpdatePublisherDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotNull().Length(1, 150);
        }
    }
}
