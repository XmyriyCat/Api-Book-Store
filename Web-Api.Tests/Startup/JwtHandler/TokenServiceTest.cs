using DLL.Models;
using BLL.Services.Implementation;
using Microsoft.Extensions.Configuration;

namespace Web_Api.Tests.Startup.JwtHandler
{
    public class TokenServiceTest : TokenService
    {
        public TokenServiceTest(IConfiguration config) : base(config)
        {
        }

        public string CreateTokenBuyerRole(string login)
        {
            return CreateToken(login, new List<Role>
            {
                new Role
                {
                    Name = "Buyer"
                }
            });
        }

        public string CreateTokenManagerRole(string login)
        {
            return CreateToken(login, new List<Role>
            {
                new Role
                {
                    Name = "Manager"
                }
            });
        }

        public string CreateTokenAdminRole(string login)
        {
            return CreateToken(login, new List<Role>
            {
                new Role
                {
                    Name = "Admin"
                }
            });
        }
        
        private string CreateToken(string login, IEnumerable<Role> roles)
        {
            var user = new User
            {
                Login = login,
                Roles = roles.ToList()
            };

            var tokenJwt = base.CreateToken(user);

            return tokenJwt;
        }
    }
}
