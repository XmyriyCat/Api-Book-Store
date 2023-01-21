using DLL.Models;

namespace DLL.Repository.Contract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsUniqueLoginAsync(string login);

        Task<IEnumerable<User>> GetAllIncludeAsync();

        Task<User> FindIncludeAsync(int id);
    }
}
