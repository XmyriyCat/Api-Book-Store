using AutoMapper;
using BLL.DTO.Role;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation;

public class RoleCatalogService : IRoleCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public RoleCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
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
        var role = _mapper.Map<Role>(item);

        role = await _repositoryWrapper.Roles.AddAsync(role);

        await _repositoryWrapper.SaveChangesAsync();

        return role;
    }

    public async Task<Role> UpdateAsync(UpdateRoleDto item)
    {
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