using AutoMapper;
using BLL.DTO.WarehouseBook;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;

namespace BLL.Services.Implementation;

public class WarehouseBookCatalogService : IWarehouseBookCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateWarehouseBookDto> _createWarehouseBookDtoValidator;
    private readonly IValidator<UpdateWarehouseBookDto> _updateWarehouseBookDtoValidator;

    public WarehouseBookCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IValidator<CreateWarehouseBookDto> createValidator, IValidator<UpdateWarehouseBookDto> updateValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _createWarehouseBookDtoValidator = createValidator;
        _updateWarehouseBookDtoValidator = updateValidator;
    }

    public async Task<IEnumerable<WarehouseBook>> GetAllAsync()
    {
        return await _repositoryWrapper.WarehouseBooks.GetAllIncludeAsync();
    }

    public async Task<WarehouseBook> FindAsync(int id)
    {
        return await _repositoryWrapper.WarehouseBooks.FindIncludeAsync(id);
    }

    public async Task<WarehouseBook> AddAsync(CreateWarehouseBookDto item)
    {
        await _createWarehouseBookDtoValidator.ValidateAndThrowAsync(item);

        var warehouseBook = _mapper.Map<WarehouseBook>(item);
        
        var bookDb = await _repositoryWrapper.Books.FindAsync(item.BookId);
        
        warehouseBook.Book = bookDb;

        var warehouseDb = await _repositoryWrapper.Warehouses.FindAsync(item.WarehouseId);
        
        warehouseBook.Warehouse = warehouseDb;

        warehouseBook = await _repositoryWrapper.WarehouseBooks.AddAsync(warehouseBook);

        await _repositoryWrapper.SaveChangesAsync();

        return warehouseBook;
    }

    public async Task<WarehouseBook> UpdateAsync(UpdateWarehouseBookDto item)
    {
        await _updateWarehouseBookDtoValidator.ValidateAndThrowAsync(item);

        var warehouseBook = _mapper.Map<WarehouseBook>(item);

        var bookDb = await _repositoryWrapper.Books.FindIncludeAsync(item.BookId);
        
        warehouseBook.Book = bookDb;

        var warehouseDb = await _repositoryWrapper.Warehouses.FindAsync(item.WarehouseId);
        
        warehouseBook.Warehouse = warehouseDb;

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