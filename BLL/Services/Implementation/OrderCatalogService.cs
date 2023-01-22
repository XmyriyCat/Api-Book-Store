using AutoMapper;
using BLL.DTO.Order;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;

namespace BLL.Services.Implementation;

public class OrderCatalogService : IOrderCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateOrderDto> _createOrderDtoValidator;
    private readonly IValidator<UpdateOrderDto> _updateOrderDtoValidator;

    public OrderCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper,
        IValidator<CreateOrderDto> createValidator, IValidator<UpdateOrderDto> updateValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _createOrderDtoValidator = createValidator;
        _updateOrderDtoValidator = updateValidator;
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
        await _createOrderDtoValidator.ValidateAndThrowAsync(item);

        var order = _mapper.Map<Order>(item);

        var shipmentDb = await _repositoryWrapper.Shipments.FindIncludeAsync(item.ShipmentId);

        if (shipmentDb is null)
        {
            throw new ValidationException($"DTO contains a non-existent shipment id.");
        }

        order.Shipment = shipmentDb;

        var customerDb = await _repositoryWrapper.Users.FindIncludeAsync(item.CustomerId);

        if (customerDb is null)
        {
            throw new ValidationException($"DTO contains a non-existent user id.");
        }

        order.User = customerDb;

        order = await _repositoryWrapper.Orders.AddAsync(order);

        await _repositoryWrapper.SaveChangesAsync();

        return order;
    }

    public async Task<Order> UpdateAsync(UpdateOrderDto item)
    {
        await _updateOrderDtoValidator.ValidateAndThrowAsync(item);

        var order = _mapper.Map<Order>(item);

        var shipmentDb = await _repositoryWrapper.Shipments.FindIncludeAsync(item.ShipmentId);

        if (shipmentDb is null)
        {
            throw new ValidationException($"DTO contains a non-existent shipment id.");
        }

        order.Shipment = shipmentDb;

        var customerDb = await _repositoryWrapper.Users.FindIncludeAsync(item.CustomerId);

        if (customerDb is null)
        {
            throw new ValidationException($"DTO contains a non-existent user id.");
        }

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