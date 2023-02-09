using AutoMapper;
using BLL.DTO.User;
using BLL.Errors;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services.Implementation
{
    public class UserCatalogService : IUserCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public UserCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITokenService tokenService, UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<AuthorizedUserDto> RegisterAsync(RegistrationUserDto item)
        {
            var isUniqueLogin = await _repositoryWrapper.Users.IsUniqueLoginAsync(item.Login);

            if (!isUniqueLogin)
            {
                throw new InvalidUserLoginError($"Login: '{item.Login}' is already used!");
            }

            var user = _mapper.Map<User>(item);

            var userRole = await _repositoryWrapper.Roles.FirstOrDefaultAsync(x => x.Name == "Buyer");
            user.Roles.Add(userRole);

            var result = await _userManager.CreateAsync(user, item.Password);

            if (!result.Succeeded)
            {
                throw new CreateIdentityUserException(result.ToString());
            }

            var authorizedUser = _mapper.Map<AuthorizedUserDto>(user);
            authorizedUser.JwtToken = _tokenService.CreateToken(user);

            return authorizedUser;
        }

        public async Task<AuthorizedUserDto> LoginAsync(LoginUserDto item)
        {
            var user = await _repositoryWrapper.Users.FirstOrDefaultAsync(x => x.Login == item.Login);

            if (user is null)
            {
                throw new UserLoginIsNotFound($"Login: '{item.Login}' is not found in database!");
            }

            var result = await _userManager.CheckPasswordAsync(user, item.Password);

            if (!result)
            {
                throw new WrongUserPasswordError($"Wrong password!");
            }

            var authorizedUser = _mapper.Map<AuthorizedUserDto>(user);
            authorizedUser.JwtToken = _tokenService.CreateToken(user);

            return authorizedUser;
        }
    }
}
