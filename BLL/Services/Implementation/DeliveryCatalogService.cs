using AutoMapper;
using BLL.DTO.Delivery;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class DeliveryCatalogService : IDeliveryCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDeliveryDto> _createDeliveryDtoValidator;
        private readonly IValidator<UpdateDeliveryDto> _updateDeliveryDtoValidator;

        public DeliveryCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IValidator<CreateDeliveryDto> createDeliveryDtoValidator, IValidator<UpdateDeliveryDto> updateDeliveryDtoValidator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _createDeliveryDtoValidator = createDeliveryDtoValidator;
            _updateDeliveryDtoValidator = updateDeliveryDtoValidator;
        }
        public async Task<Delivery> FindAsync(int id)
        {
            return await _repositoryWrapper.Deliveries.FindAsync(id);
        }

        public async Task<IEnumerable<Delivery>> GetAllAsync()
        {
            return await _repositoryWrapper.Deliveries.GetAll().ToListAsync();
        }

        public async Task<Delivery> AddAsync(CreateDeliveryDto item)
        {
            await _createDeliveryDtoValidator.ValidateAndThrowAsync(item);

            var delivery = _mapper.Map<Delivery>(item);

            delivery = await _repositoryWrapper.Deliveries.AddAsync(delivery);

            await _repositoryWrapper.SaveChangesAsync();

            return delivery;
        }

        public async Task<Delivery> UpdateAsync(UpdateDeliveryDto item)
        {
            await _updateDeliveryDtoValidator.ValidateAndThrowAsync(item);

            var delivery = _mapper.Map<Delivery>(item);

            delivery = await _repositoryWrapper.Deliveries.UpdateAsync(delivery.Id, delivery);

            await _repositoryWrapper.SaveChangesAsync();

            return delivery;
        }

        public async Task DeleteAsync(int id)
        {
            await _repositoryWrapper.Deliveries.DeleteAsync(id);
            
            await _repositoryWrapper.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _repositoryWrapper.Deliveries.CountAsync();
        }
    }
}
