using AutoMapper;
using BLL.DTO.User;
using BLL.Errors;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services.Implementation
{
    public class UserGoogleCatalogService : IUserGoogleCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IGoogleTokenService _googleTokenService;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public UserGoogleCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IGoogleTokenService googleTokenService, UserManager<User> userManager, ITokenService tokenService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _googleTokenService = googleTokenService;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthorizedUserDto> RegisterGoogleAsync(RegistrationGoogleUserDto registrationGoogleUserDto)
        {
            var payload = await _googleTokenService.ValidateGoogleTokenAsync(registrationGoogleUserDto.GoogleToken);

            var user = _mapper.Map<User>(payload);

            var isUniqueLogin = await _repositoryWrapper.Users.IsUniqueLoginAsync(user.Login);

            if (!isUniqueLogin)
            {
                throw new InvalidUserLoginError($"Login: '{user.Login}' is already used!");
            }

            var userRole = await _repositoryWrapper.Roles.FirstOrDefaultAsync(x => x.Name == "Buyer");
            user.Roles.Add(userRole);

            var result = await _userManager.CreateAsync(user, registrationGoogleUserDto.Password);

            if (!result.Succeeded)
            {
                throw new CreateIdentityUserException(result.ToString());
            }

            var authorizedUser = _mapper.Map<AuthorizedUserDto>(user);
            authorizedUser.JwtToken = _tokenService.CreateToken(user);

            return authorizedUser;
        }

        public async Task<AuthorizedUserDto> LoginGoogleAsync(LoginGoogleUserDto loginGoogleUserDto)
        {
            var payload = await _googleTokenService.ValidateGoogleTokenAsync(loginGoogleUserDto.GoogleToken);

            var user = _mapper.Map<User>(payload);

            var userDb = await _repositoryWrapper.Users.FirstOrDefaultAsync(x => x.Login == user.Login);

            if (userDb is null)
            {
                throw new UserLoginIsNotFound($"Login: '{user.Login}' is not found in database!");
            }

            var authorizedUser = _mapper.Map<AuthorizedUserDto>(userDb);
            authorizedUser.JwtToken = _tokenService.CreateToken(userDb);

            return authorizedUser;
        }
    }
}
