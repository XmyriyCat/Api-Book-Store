using BLL.DTO.User;
using DLL.Models;

namespace BLL.Services.Contract
{
    public interface IUserCatalogService
    {
        Task<User> RegisterAsync(RegistrationUserDto item);

        Task<User> LoginAsync(LoginUserDto item);

        Task<bool> IsUniqueLoginAsync(string login);
    }
}
