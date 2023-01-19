using FluentValidation;
using Google.Apis.Auth;

namespace BLL.Infrastructure.Validators.GooglePayload
{
    public class GooglePayloadValidator : AbstractValidator<GoogleJsonWebSignature.Payload>
    {
        public GooglePayloadValidator()
        {
            RuleFor(x => x.Name).Length(1, 150);
            RuleFor(x => x.Email).EmailAddress().Length(1, 150);
            RuleFor(x => x.EmailVerified).NotEmpty();
        }
    }
}
