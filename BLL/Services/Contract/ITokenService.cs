using DLL.Models;
using Google.Apis.Auth;

namespace BLL.Services.Contract
{
    public interface ITokenService
    {   
        string CreateToken(User user);
    }
}
