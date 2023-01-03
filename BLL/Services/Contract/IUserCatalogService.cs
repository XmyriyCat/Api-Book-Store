using BLL.DTO.User;
using DLL.Models;

namespace BLL.Services.Contract
{
    public interface IUserCatalogService
    {
        Task<User> RegisterAsync(CreateUserDto item);

        Task<User> LoginAsync(CreateUserDto item);

        Task<bool> IsUniqueLoginAsync(string login);
    }
}
