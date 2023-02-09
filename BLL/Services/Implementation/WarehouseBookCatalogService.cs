using AutoMapper;
using BLL.DTO.WarehouseBook;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;

namespace BLL.Services.Implementation;

public class WarehouseBookCatalogService : IWarehouseBookCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public WarehouseBookCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
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