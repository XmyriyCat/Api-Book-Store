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
        private readonly IValidator<CreateUserDto> _createUserDtoValidator;

        public UserCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IValidator<CreateUserDto> createValidator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _createUserDtoValidator = createValidator;
        }

        public async Task<User> RegisterAsync(CreateUserDto item)
        {
            await _createUserDtoValidator.ValidateAndThrowAsync(item);

            if (!await IsUniqueLoginAsync(item.Login))
            {
                throw new Exception($"Login:{item.Login} is already used!"); // TODO create new Exception class for this exception!!!!!!!!!!!
            }

            var user = _mapper.Map<User>(item);

            var hmac = new HMACSHA512();

            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(item.Password));
            user.PasswordSalt = hmac.Key;

            await _repositoryWrapper.Users.AddAsync(user);
            await _repositoryWrapper.SaveChangesAsync();

            return user;
        }

        public async Task<User> LoginAsync(CreateUserDto item)
        {
            await _createUserDtoValidator.ValidateAndThrowAsync(item);

            var user = await _repositoryWrapper.Users.SingleOrDefaultAsync(x => x.Login == item.Login);

            if (user is null)
            {
                throw new Exception($"Login:{item.Login} is not found in database"); // TODO create new Exception class for this exception!!!!!!!!!!!
            }

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(item.Password));

            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    throw new Exception("Invalid Password"); // TODO create new Exception class for this exception!!!!!!!!!!!
                }
            }

            return user;
        }

        public async Task<bool> IsUniqueLoginAsync(string login)
        {
            return await _repositoryWrapper.Users.IsUniqueLoginAsync(login);
        }
    }
}
