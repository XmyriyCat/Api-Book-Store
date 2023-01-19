using AutoMapper;
using BLL.DTO.OrderLine;
using BLL.DTO.Shipment;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

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
        return await _repositoryWrapper.Shipments.GetAll().ToListAsync();
    }

    public async Task<Shipment> FindAsync(int id)
    {
        return await _repositoryWrapper.Shipments.FindAsync(id);
    }

    public async Task<Shipment> AddAsync(CreateShipmentDto item)
    {
        await _createShipmentDtoValidator.ValidateAndThrowAsync(item);
        var shipment = _mapper.Map<Shipment>(item);

        shipment = await _repositoryWrapper.Shipments.AddAsync(shipment);

        await _repositoryWrapper.SaveChangesAsync();

        return shipment;
    }

    public async Task<Shipment> UpdateAsync(UpdateShipmentDto item)
    {
        await _updateShipmentDtoValidator.ValidateAndThrowAsync(item);

        var shipment = _mapper.Map<Shipment>(item);

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