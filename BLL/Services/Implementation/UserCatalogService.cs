using AutoMapper;
using BLL.DTO.User;
using BLL.Errors;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services.Implementation
{
    public class UserCatalogService : IUserCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IValidator<RegistrationUserDto> _registrationUserDtoValidator;
        private readonly IValidator<LoginUserDto> _loginUserDtoValidator;
        private readonly ITokenService _tokenService;
        private readonly IGoogleTokenService _googleTokenService;
        private readonly IValidator<GoogleJsonWebSignature.Payload> _googlePayloadValidator;
        private readonly IValidator<RegistrationGoogleUserDto> _registrationGoogleUserDtoValidator;
        private readonly IValidator<LoginGoogleUserDto> _loginGoogleUserDtoValidator;
        private readonly UserManager<User> _userManager;

        public UserCatalogService(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IValidator<LoginUserDto> loginValidator,
            IValidator<RegistrationUserDto> registrationValidator,
            ITokenService tokenService,
            IGoogleTokenService googleTokenService,
            IValidator<GoogleJsonWebSignature.Payload> googlePayloadValidator,
            IValidator<RegistrationGoogleUserDto> registrationGoogleUserDtoValidator,
            IValidator<LoginGoogleUserDto> loginGoogleUserDtoValidator,
            UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _loginUserDtoValidator = loginValidator;
            _registrationUserDtoValidator = registrationValidator;
            _tokenService = tokenService;
            _googleTokenService = googleTokenService;
            _googlePayloadValidator = googlePayloadValidator;
            _registrationGoogleUserDtoValidator = registrationGoogleUserDtoValidator;
            _loginGoogleUserDtoValidator = loginGoogleUserDtoValidator;
            _userManager = userManager;
        }

        public async Task<AuthorizedUserDto> RegisterAsync(RegistrationUserDto item)
        {
            await _registrationUserDtoValidator.ValidateAndThrowAsync(item);

            if (!await IsUniqueLoginAsync(item.Login))
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
            await _loginUserDtoValidator.ValidateAndThrowAsync(item);

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
        
        public async Task<AuthorizedUserDto> RegisterGoogleAsync(RegistrationGoogleUserDto registrationGoogleUserDto)
        {
            await _registrationGoogleUserDtoValidator.ValidateAndThrowAsync(registrationGoogleUserDto);

            var payload = await _googleTokenService.ValidateGoogleTokenAsync(registrationGoogleUserDto.GoogleToken);

            await _googlePayloadValidator.ValidateAndThrowAsync(payload);

            var user = _mapper.Map<User>(payload);

            if (!await IsUniqueLoginAsync(user.Login))
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
            await _loginGoogleUserDtoValidator.ValidateAndThrowAsync(loginGoogleUserDto);

            var payload = await _googleTokenService.ValidateGoogleTokenAsync(loginGoogleUserDto.GoogleToken);

            await _googlePayloadValidator.ValidateAndThrowAsync(payload);

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

        public async Task<bool> IsUniqueLoginAsync(string login)
        {
            return await _repositoryWrapper.Users.IsUniqueLoginAsync(login);
        }
    }
}
