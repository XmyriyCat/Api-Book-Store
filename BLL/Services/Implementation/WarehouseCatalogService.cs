using AutoMapper;
using BLL.DTO.Warehouse;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation;

public class WarehouseCatalogService : IWarehouseCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public WarehouseCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<Warehouse>> GetAllAsync()
    {
        return await _repositoryWrapper.Warehouses.GetAll().ToListAsync();
    }

    public async Task<Warehouse> FindAsync(int id)
    {
        return await _repositoryWrapper.Warehouses.FindAsync(id);
    }
    
    public async Task<Warehouse> AddAsync(CreateWarehouseDto item)
    {
        var warehouse = _mapper.Map<Warehouse>(item);
    
        warehouse = await _repositoryWrapper.Warehouses.AddAsync(warehouse);
    
        await _repositoryWrapper.SaveChangesAsync();
    
        return warehouse;
    }
    
    public async Task<Warehouse> UpdateAsync(UpdateWarehouseDto item)
    {
        var warehouse = _mapper.Map<Warehouse>(item);
    
        warehouse = await _repositoryWrapper.Warehouses.UpdateAsync(warehouse.Id, warehouse);
    
        await _repositoryWrapper.SaveChangesAsync();
    
        return warehouse;
    }
    
    public async Task DeleteAsync(int id)
    { 
        await _repositoryWrapper.Warehouses.DeleteAsync(id);
    
        await _repositoryWrapper.SaveChangesAsync();
    }
    
    public async Task<int> CountAsync()
    {
        return await _repositoryWrapper.Warehouses.CountAsync();
    }
}