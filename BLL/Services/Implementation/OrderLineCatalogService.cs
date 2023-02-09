using AutoMapper;
using BLL.DTO.OrderLine;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;

namespace BLL.Services.Implementation;

public class OrderLineCatalogService : IOrderLineCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public OrderLineCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderLine>> GetAllAsync()
    {
        return await _repositoryWrapper.OrderLines.GetAllIncludeAsync();
    }

    public async Task<OrderLine> FindAsync(int id)
    {
        return await _repositoryWrapper.OrderLines.FindIncludeAsync(id);
    }

    public async Task<OrderLine> AddAsync(CreateOrderLineDto item)
    {
        var orderLine = _mapper.Map<OrderLine>(item);

        var warehouseBookDb = await _repositoryWrapper.WarehouseBooks.FindIncludeAsync(item.WarehouseBookId);
        
        orderLine.WarehouseBook = warehouseBookDb;

        var orderDb = await _repositoryWrapper.Orders.FindIncludeAsync(item.OrderId);
        
        orderLine.Order = orderDb;

        orderLine = await _repositoryWrapper.OrderLines.AddAsync(orderLine);

        await _repositoryWrapper.SaveChangesAsync();

        return orderLine;
    }

    public async Task<OrderLine> UpdateAsync(UpdateOrderLineDto item)
    {
        var orderLine = _mapper.Map<OrderLine>(item);

        var warehouseBookDb = await _repositoryWrapper.WarehouseBooks.FindIncludeAsync(item.WarehouseBookId);
        
        orderLine.WarehouseBook = warehouseBookDb;

        var orderDb = await _repositoryWrapper.Orders.FindIncludeAsync(item.OrderId);
        
        orderLine.Order = orderDb;

        orderLine = await _repositoryWrapper.OrderLines.UpdateAsync(orderLine.Id, orderLine);

        await _repositoryWrapper.SaveChangesAsync();

        return orderLine;
    }

    public async Task DeleteAsync(int id)
    {
        await _repositoryWrapper.OrderLines.DeleteAsync(id);

        await _repositoryWrapper.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _repositoryWrapper.OrderLines.CountAsync();
    }
}