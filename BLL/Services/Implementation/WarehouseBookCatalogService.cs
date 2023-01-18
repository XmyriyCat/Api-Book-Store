using AutoMapper;
using BLL.DTO.WarehouseBook;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation;

public class WarehouseBookCatalogService : IWarehouseBookCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateWarehouseBookDto> _createPaymentWayDtoValidator;
    private readonly IValidator<UpdateWarehouseBookDto> _updatePaymentWayDtoValidator;

    public WarehouseBookCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IValidator<CreateWarehouseBookDto> createValidator, IValidator<UpdateWarehouseBookDto> updateValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _createPaymentWayDtoValidator = createValidator;
        _updatePaymentWayDtoValidator = updateValidator;
    }

    public async Task<IEnumerable<WarehouseBook>> GetAllAsync()
    {
        return await _repositoryWrapper.WarehouseBooks.GetAll().ToListAsync();
    }

    public async Task<WarehouseBook> FindAsync(int id)
    {
        return await _repositoryWrapper.WarehouseBooks.FindAsync(id);
    }

    public async Task<WarehouseBook> AddAsync(CreateWarehouseBookDto item)
    {
        await _createPaymentWayDtoValidator.ValidateAndThrowAsync(item);
        var warehouseBook = _mapper.Map<WarehouseBook>(item);

        warehouseBook = await _repositoryWrapper.WarehouseBooks.AddAsync(warehouseBook);

        await _repositoryWrapper.SaveChangesAsync();

        return warehouseBook;
    }

    public async Task<WarehouseBook> UpdateAsync(UpdateWarehouseBookDto item)
    {
        await _updatePaymentWayDtoValidator.ValidateAndThrowAsync(item);

        var warehouseBook = _mapper.Map<WarehouseBook>(item);

        warehouseBook = await _repositoryWrapper.WarehouseBooks.UpdateAsync(warehouseBook.Id, warehouseBook);

        await _repositoryWrapper.SaveChangesAsync();

        return warehouseBook;
    }

    public async Task DeleteAsync(int id)
    {
        await _repositoryWrapper.WarehouseBooks.DeleteAsync(id);

        await _repositoryWrapper.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _repositoryWrapper.WarehouseBooks.CountAsync();
    }
}