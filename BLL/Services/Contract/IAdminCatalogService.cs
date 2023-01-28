using BLL.DTO.User;
using DLL.Models;

namespace BLL.Services.Contract;

public interface IAdminCatalogService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> FindAsync(int id);
    Task<User> AddAsync(CreateUserDto item);
    Task<User> UpdateAsync(UpdateUserDto item);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}