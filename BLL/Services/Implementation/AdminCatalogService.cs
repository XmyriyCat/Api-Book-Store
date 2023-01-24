using AutoMapper;
using BLL.DTO.User;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation;

public class AdminCatalogService : IAdminCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateUserDto> _createUserDtoValidator;
    private readonly IValidator<UpdateUserDto> _updateUserDtoValidator;
    
    public AdminCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, 
                                IValidator<CreateUserDto> createUserDtoValidator, IValidator<UpdateUserDto> updateUserDtoValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _createUserDtoValidator = createUserDtoValidator;
        _updateUserDtoValidator = updateUserDtoValidator;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _repositoryWrapper.Users.GetAll().ToListAsync();
    }

    public async Task<User> FindAsync(int id)
    {
        return await _repositoryWrapper.Users.FindAsync(id);
    }

    public async Task<User> AddAsync(CreateUserDto item)
    {
        await _createUserDtoValidator.ValidateAndThrowAsync(item);

        var user = _mapper.Map<User>(item);

        user = await _repositoryWrapper.Users.AddAsync(user);

        await _repositoryWrapper.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateAsync(UpdateUserDto item)
    {
        await _updateUserDtoValidator.ValidateAndThrowAsync(item);

        var user = _mapper.Map<User>(item);

        user = await _repositoryWrapper.Users.UpdateAsync(user.Id, user);

        await _repositoryWrapper.SaveChangesAsync();

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