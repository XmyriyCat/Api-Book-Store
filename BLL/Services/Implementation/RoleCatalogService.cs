using AutoMapper;
using BLL.DTO.Role;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation;

public class RoleCatalogService : IRoleCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateRoleDto> _createRoleDtoValidator;
    private readonly IValidator<UpdateRoleDto> _updateRoleDtoValidator;

    public RoleCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper,
        IValidator<CreateRoleDto> createValidator, IValidator<UpdateRoleDto> updateValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _createRoleDtoValidator = createValidator;
        _updateRoleDtoValidator = updateValidator;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _repositoryWrapper.Roles.GetAll().ToListAsync();
    }

    public async Task<Role> FindAsync(int id)
    {
        return await _repositoryWrapper.Roles.FindAsync(id);
    }

    public async Task<Role> AddAsync(CreateRoleDto item)
    {
        await _createRoleDtoValidator.ValidateAndThrowAsync(item);
        var role = _mapper.Map<Role>(item);

        role = await _repositoryWrapper.Roles.AddAsync(role);

        await _repositoryWrapper.SaveChangesAsync();

        return role;
    }

    public async Task<Role> UpdateAsync(UpdateRoleDto item)
    {
        await _updateRoleDtoValidator.ValidateAndThrowAsync(item);

        var role = _mapper.Map<Role>(item);

        role = await _repositoryWrapper.Roles.UpdateAsync(role.Id, role);

        await _repositoryWrapper.SaveChangesAsync();

        return role;
    }

    public async Task DeleteAsync(int id)
    {
        await _repositoryWrapper.Roles.DeleteAsync(id);

        await _repositoryWrapper.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _repositoryWrapper.Roles.CountAsync();
    }
}