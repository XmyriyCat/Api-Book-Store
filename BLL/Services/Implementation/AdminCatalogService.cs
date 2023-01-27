using AutoMapper;
using BLL.DTO.User;
using BLL.Errors;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services.Implementation;

public class AdminCatalogService : IAdminCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateUserDto> _createUserDtoValidator;
    private readonly IValidator<UpdateUserDto> _updateUserDtoValidator;
    private readonly UserManager<User> _userManager;

    public AdminCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper,
                                IValidator<CreateUserDto> createUserDtoValidator, IValidator<UpdateUserDto> updateUserDtoValidator, UserManager<User> userManager)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _createUserDtoValidator = createUserDtoValidator;
        _updateUserDtoValidator = updateUserDtoValidator;
        _userManager = userManager;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _repositoryWrapper.Users.GetAllIncludeAsync();
    }

    public async Task<User> FindAsync(int id)
    {
        return await _repositoryWrapper.Users.FindIncludeAsync(id);
    }

    public async Task<User> AddAsync(CreateUserDto item)
    {
        await _createUserDtoValidator.ValidateAndThrowAsync(item);

        var user = _mapper.Map<User>(item);

        user.Roles = new HashSet<Role>();
        user.Orders = new HashSet<Order>();

        foreach (var idRole in item.RolesIds)
        {
            var role = await _repositoryWrapper.Roles.FindAsync(idRole);

            if (role is null)
            {
                throw new ValidationException($"DTO contains a non-existent role id.");
            }

            user.Roles.Add(role);
        }

        var result = await _userManager.CreateAsync(user, item.Password);

        if (!result.Succeeded)
        {
            throw new CreateIdentityUserException(result.ToString());
        }

        return user;
    }

    public async Task<User> UpdateAsync(UpdateUserDto item)
    {
        await _updateUserDtoValidator.ValidateAndThrowAsync(item);

        var user = await _repositoryWrapper.Users.FirstOrDefaultAsync(x => x.Id == item.Id);

        user.UserName = item.Username;
        user.Login = item.Login;
        user.Email = item.Email;
        user.City = item.City;
        user.Address = item.Address;

        user.Roles = new HashSet<Role>();
        user.Orders = new HashSet<Order>();

        foreach (var idRole in item.RolesIds)
        {
            var role = await _repositoryWrapper.Roles.FindAsync(idRole);

            if (role is null)
            {
                throw new ValidationException($"DTO contains a non-existent role id.");
            }

            user.Roles.Add(role);
        }

        foreach (var idOrder in item.OrderIds)
        {
            var order = await _repositoryWrapper.Orders.FindAsync(idOrder);

            if (order is null)
            {
                throw new ValidationException($"DTO contains a non-existent order id.");
            }

            user.Orders.Add(order);
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new CreateIdentityUserException(result.ToString());
        }

        return user;
    }

    public async Task DeleteAsync(int id)
    {
        await _repositoryWrapper.Users.DeleteAsync(id);

        await _repositoryWrapper.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _repositoryWrapper.Users.CountAsync();
    }
}