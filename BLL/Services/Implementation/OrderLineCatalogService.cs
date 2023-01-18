using AutoMapper;
using BLL.DTO.OrderLine;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation;

public class OrderLineCatalogService : IOrderLineCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateOrderLineDto> _createPaymentWayDtoValidator;
    private readonly IValidator<UpdateOrderLineDto> _updatePaymentWayDtoValidator;

    public OrderLineCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper,
        IValidator<CreateOrderLineDto> createValidator, IValidator<UpdateOrderLineDto> updateValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _createPaymentWayDtoValidator = createValidator;
        _updatePaymentWayDtoValidator = updateValidator;
    }

    public async Task<IEnumerable<OrderLine>> GetAllAsync()
    {
        return await _repositoryWrapper.OrderLines.GetAll().ToListAsync();
    }

    public async Task<OrderLine> FindAsync(int id)
    {
        return await _repositoryWrapper.OrderLines.FindAsync(id);
    }

    public async Task<OrderLine> AddAsync(CreateOrderLineDto item)
    {
        await _createPaymentWayDtoValidator.ValidateAndThrowAsync(item);
        var orderLine = _mapper.Map<OrderLine>(item);

        orderLine = await _repositoryWrapper.OrderLines.AddAsync(orderLine);

        await _repositoryWrapper.SaveChangesAsync();

        return orderLine;
    }

    public async Task<OrderLine> UpdateAsync(UpdateOrderLineDto item)
    {
        await _updatePaymentWayDtoValidator.ValidateAndThrowAsync(item);

        var orderLine = _mapper.Map<OrderLine>(item);

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