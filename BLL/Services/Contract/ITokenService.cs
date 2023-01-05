using DLL.Models;

namespace BLL.Services.Contract
{
    public interface ITokenService
    {   
        string CreateToken(User user);
    }
}
