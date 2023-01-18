using AutoMapper;
using BLL.DTO.PaymentWay;
using BLL.DTO.Warehouse;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation;

public class WarehouseCatalogService : IWarehouseCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateWarehouseDto> _createPaymentWayDtoValidator;
    private readonly IValidator<UpdateWarehouseDto> _updatePaymentWayDtoValidator;
    
    public WarehouseCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IValidator<CreateWarehouseDto> createValidator, IValidator<UpdateWarehouseDto> updateValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _createPaymentWayDtoValidator = createValidator;
        _updatePaymentWayDtoValidator = updateValidator;
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
        await _createPaymentWayDtoValidator.ValidateAndThrowAsync(item);
        var warehouse = _mapper.Map<Warehouse>(item);
    
        warehouse = await _repositoryWrapper.Warehouses.AddAsync(warehouse);
    
        await _repositoryWrapper.SaveChangesAsync();
    
        return warehouse;
    }
    
    public async Task<Warehouse> UpdateAsync(UpdateWarehouseDto item)
    {
        await _updatePaymentWayDtoValidator.ValidateAndThrowAsync(item);
    
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