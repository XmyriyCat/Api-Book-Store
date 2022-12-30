using AutoMapper;
using BLL.DTO.Publisher;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class PublisherCatalogService : IPublisherCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePublisherDto> _createPublisherDtoValidator;
        private readonly IValidator<UpdatePublisherDto> _updatePublisherDtoValidator;

        public PublisherCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IValidator<CreatePublisherDto> createValidator, IValidator<UpdatePublisherDto> updateValidator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _createPublisherDtoValidator = createValidator;
            _updatePublisherDtoValidator = updateValidator;
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            return await _repositoryWrapper.Publishers.GetAll().ToListAsync();
        }

        public async Task<Publisher> FindAsync(int id)
        {
            return await _repositoryWrapper.Publishers.FindAsync(id);
        }

        public async Task<Publisher> AddAsync(CreatePublisherDto item)
        {
            var validationResult = await _createPublisherDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }

            var publisher = _mapper.Map<Publisher>(item);

            await _repositoryWrapper.Publishers.AddAsync(publisher);

            await _repositoryWrapper.SaveChangesAsync();

            return publisher;
        }

        public async Task<Publisher> UpdateAsync(UpdatePublisherDto item)
        {
            var validationResult = await _updatePublisherDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }

            var publisher = _mapper.Map<Publisher>(item);

            await _repositoryWrapper.Publishers.UpdateAsync(publisher.Id, publisher);

            await _repositoryWrapper.SaveChangesAsync();

            return publisher;
        }

        public async Task DeleteAsync(int id)
        {
            await _repositoryWrapper.Publishers.DeleteAsync(id);

            await _repositoryWrapper.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _repositoryWrapper.Publishers.CountAsync();
        }
    }
}

