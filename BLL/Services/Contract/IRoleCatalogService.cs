using BLL.DTO.Role;
using DLL.Models;

namespace BLL.Services.Contract;

public interface IRoleCatalogService
{
    Task<IEnumerable<Role>> GetAllAsync();
    Task<Role> FindAsync(int id);
    Task<Role> AddAsync(CreateRoleDto item);
    Task<Role> UpdateAsync(UpdateRoleDto item);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}