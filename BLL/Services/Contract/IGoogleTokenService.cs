using Google.Apis.Auth;

namespace BLL.Services.Contract
{
    public interface IGoogleTokenService
    {
        Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string googleIdToken);
    }
}
