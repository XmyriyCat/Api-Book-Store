using DLL.Models;

namespace DLL.Repository.Contract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsUniqueLoginAsync(string login);
    }
}
