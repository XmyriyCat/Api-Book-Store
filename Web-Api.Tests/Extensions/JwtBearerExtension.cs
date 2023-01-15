using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Web_Api.Tests.Extensions
{
    public static class JwtBearerExtension
    {
        public static void AddJwtToken(this HttpClient client, string tokenJwt)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, tokenJwt);
        }
    }
}
