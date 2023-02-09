using AutoMapper;
using BLL.DTO.Order;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;

namespace BLL.Services.Implementation;

public class OrderCatalogService : IOrderCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public OrderCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _repositoryWrapper.Orders.GetAllIncludeAsync();
    }

    public async Task<Order> FindAsync(int id)
    {
        return await _repositoryWrapper.Orders.FindIncludeAsync(id);
    }

    public async Task<Order> AddAsync(CreateOrderDto item)
    {
        var order = _mapper.Map<Order>(item);

        var shipmentDb = await _repositoryWrapper.Shipments.FindIncludeAsync(item.ShipmentId);

        order.Shipment = shipmentDb;

        var customerDb = await _repositoryWrapper.Users.FindIncludeAsync(item.CustomerId);

        order.User = customerDb;

        order = await _repositoryWrapper.Orders.AddAsync(order);

        await _repositoryWrapper.SaveChangesAsync();

        return order;
    }

    public async Task<Order> UpdateAsync(UpdateOrderDto item)
    {
        var order = _mapper.Map<Order>(item);

        var shipmentDb = await _repositoryWrapper.Shipments.FindIncludeAsync(item.ShipmentId);

        order.Shipment = shipmentDb;

        var customerDb = await _repositoryWrapper.Users.FindIncludeAsync(item.CustomerId);

        order.User = customerDb;

        order = await _repositoryWrapper.Orders.UpdateAsync(order.Id, order);

        await _repositoryWrapper.SaveChangesAsync();

        return order;
    }

    public async Task DeleteAsync(int id)
    {
        await _repositoryWrapper.Orders.DeleteAsync(id);

        await _repositoryWrapper.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _repositoryWrapper.Orders.CountAsync();
    }
}