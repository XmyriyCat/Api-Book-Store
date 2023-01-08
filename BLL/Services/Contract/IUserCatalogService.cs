using BLL.DTO.User;

namespace BLL.Services.Contract
{
    public interface IUserCatalogService
    {
        Task<AuthorizedUserDto> RegisterAsync(RegistrationUserDto item);

        Task<AuthorizedUserDto> LoginAsync(LoginUserDto item);

        Task<bool> IsUniqueLoginAsync(string login);
    }
}
