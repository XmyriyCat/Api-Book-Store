using BLL.DTO.User;

namespace BLL.Services.Contract
{
    public interface IUserGoogleCatalogService
    {
        Task<AuthorizedUserDto> RegisterGoogleAsync(RegistrationGoogleUserDto item);

        Task<AuthorizedUserDto> LoginGoogleAsync(LoginGoogleUserDto item);
    }
}
