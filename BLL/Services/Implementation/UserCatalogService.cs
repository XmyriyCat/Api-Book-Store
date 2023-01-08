using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using BLL.DTO.User;
using BLL.Errors;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;

namespace BLL.Services.Implementation
{
    public class UserCatalogService : IUserCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IValidator<RegistrationUserDto> _registrationUserDtoValidator;
        private readonly IValidator<LoginUserDto> _loginUserDtoValidator;
        private readonly ITokenService _tokenService;

        public UserCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IValidator<LoginUserDto> loginValidator, IValidator<RegistrationUserDto> registrationValidator, ITokenService tokenService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _loginUserDtoValidator = loginValidator;
            _registrationUserDtoValidator = registrationValidator;
            _tokenService = tokenService;
        }

        public async Task<AuthorizedUserDto> RegisterAsync(RegistrationUserDto item)
        {
            await _registrationUserDtoValidator.ValidateAndThrowAsync(item);

            if (!await IsUniqueLoginAsync(item.Login))
            {
                throw new InvalidUserLoginError($"Login: '{item.Login}' is already used!"); // TODO generate 409 status code!!!!!!!!!!!
            }

            var user = _mapper.Map<User>(item);
            
            var userRole = await _repositoryWrapper.Roles.FirstOrDefaultAsync(x => x.Name == "Buyer");
            user.Roles.Add(userRole);

            var hmac = new HMACSHA512();

            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(item.Password));
            user.PasswordSalt = hmac.Key;

            await _repositoryWrapper.Users.AddAsync(user);
            await _repositoryWrapper.SaveChangesAsync();

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
                throw new UserLoginIsNotFound($"Login: '{item.Login}' is not found in database!"); // TODO generate 401 status code!!!!!!!!!!!
            }

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(item.Password));
            
            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    throw new WrongUserPasswordError("Wrong password!"); // TODO generate 401 status code!!!!!!!!!!!
                }
            }

            var authorizedUser = _mapper.Map<AuthorizedUserDto>(user);
            authorizedUser.JwtToken = _tokenService.CreateToken(user);

            return authorizedUser;
        }

        public async Task<bool> IsUniqueLoginAsync(string login)
        {
            return await _repositoryWrapper.Users.IsUniqueLoginAsync(login);
        }
    }
}
