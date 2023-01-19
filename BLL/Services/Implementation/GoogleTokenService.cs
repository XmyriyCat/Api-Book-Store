using BLL.Services.Contract;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace BLL.Services.Implementation
{
    public class GoogleTokenService : IGoogleTokenService
    {
        private readonly IConfiguration _config;

        public GoogleTokenService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string googleIdToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string>
                {
                    _config["Authentication:Google:ClientId"]
                }
            };
            
            var payload = await GoogleJsonWebSignature.ValidateAsync(googleIdToken, settings);

            return payload;
        }
    }
}
