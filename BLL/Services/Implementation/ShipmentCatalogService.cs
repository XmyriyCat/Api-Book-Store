using AutoMapper;
using BLL.DTO.Shipment;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;

namespace BLL.Services.Implementation;

public class ShipmentCatalogService : IShipmentCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateShipmentDto> _createShipmentDtoValidator;
    private readonly IValidator<UpdateShipmentDto> _updateShipmentDtoValidator;

    public ShipmentCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper,
        IValidator<CreateShipmentDto> createValidator, IValidator<UpdateShipmentDto> updateValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _createShipmentDtoValidator = createValidator;
        _updateShipmentDtoValidator = updateValidator;
    }

    public async Task<IEnumerable<Shipment>> GetAllAsync()
    {
        return await _repositoryWrapper.Shipments.GetAllIncludeAsync();
    }

    public async Task<Shipment> FindAsync(int id)
    {
        return await _repositoryWrapper.Shipments.FindIncludeAsync(id);
    }

    public async Task<Shipment> AddAsync(CreateShipmentDto item)
    {
        await _createShipmentDtoValidator.ValidateAndThrowAsync(item);

        var shipment = _mapper.Map<Shipment>(item);

        var deliveryDb = await _repositoryWrapper.Deliveries.FindAsync(item.DeliveryId);

        if (deliveryDb is null)
        {
            throw new ValidationException($"DTO contains a non-existent delivery id.");
        }

        shipment.Delivery = deliveryDb;

        var paymentWayDb = await _repositoryWrapper.PaymentWays.FindAsync(item.PaymentWayId);

        if (paymentWayDb is null)
        {
            throw new ValidationException($"DTO contains a non-existent payment way id.");
        }

        shipment.PaymentWay = paymentWayDb;

        shipment = await _repositoryWrapper.Shipments.AddAsync(shipment);

        await _repositoryWrapper.SaveChangesAsync();

        return shipment;
    }

    public async Task<Shipment> UpdateAsync(UpdateShipmentDto item)
    {
        await _updateShipmentDtoValidator.ValidateAndThrowAsync(item);

        var shipment = _mapper.Map<Shipment>(item);

        var deliveryDb = await _repositoryWrapper.Deliveries.FindAsync(item.DeliveryId);

        if (deliveryDb is null)
        {
            throw new ValidationException($"DTO contains a non-existent delivery id.");
        }

        shipment.Delivery = deliveryDb;

        var paymentWayDb = await _repositoryWrapper.PaymentWays.FindAsync(item.PaymentWayId);

        if (paymentWayDb is null)
        {
            throw new ValidationException($"DTO contains a non-existent payment way id.");
        }

        shipment.PaymentWay = paymentWayDb;

        shipment = await _repositoryWrapper.Shipments.UpdateAsync(shipment.Id, shipment);

        await _repositoryWrapper.SaveChangesAsync();

        return shipment;
    }

    public async Task DeleteAsync(int id)
    {
        await _repositoryWrapper.Shipments.DeleteAsync(id);

        await _repositoryWrapper.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _repositoryWrapper.Shipments.CountAsync();
    }
}