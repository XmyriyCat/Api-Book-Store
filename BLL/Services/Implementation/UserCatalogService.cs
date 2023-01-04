using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using BLL.DTO.User;
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

        public async Task<User> RegisterAsync(RegistrationUserDto item)
        {
            await _registrationUserDtoValidator.ValidateAndThrowAsync(item);

            if (!await IsUniqueLoginAsync(item.Login))
            {
                throw new Exception($"Login:{item.Login} is already used!"); // TODO create new Exception class for this exception 409!!!!!!!!!!!
            }

            var user = _mapper.Map<User>(item);

            var hmac = new HMACSHA512();

            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(item.Password));
            user.PasswordSalt = hmac.Key;

            await _repositoryWrapper.Users.AddAsync(user);
            await _repositoryWrapper.SaveChangesAsync();

            Console.WriteLine("JWT token: " + _tokenService.CreateToken(user.Login));

            return user;
        }

        public async Task<User> LoginAsync(LoginUserDto item)
        {
            await _loginUserDtoValidator.ValidateAndThrowAsync(item);

            var user = await _repositoryWrapper.Users.SingleOrDefaultAsync(x => x.Login == item.Login);

            if (user is null)
            {
                throw new Exception($"Login:{item.Login} is not found in database"); // TODO create new Exception class for this exception 401!!!!!!!!!!!
            }

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(item.Password));

            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    throw new Exception("Invalid Password"); // TODO create new Exception class for this exception 401!!!!!!!!!!!
                }
            }

            Console.WriteLine("JWT token: " + _tokenService.CreateToken(user.Login));

            return user;
        }

        public async Task<bool> IsUniqueLoginAsync(string login)
        {
            return await _repositoryWrapper.Users.IsUniqueLoginAsync(login);
        }
    }
}
