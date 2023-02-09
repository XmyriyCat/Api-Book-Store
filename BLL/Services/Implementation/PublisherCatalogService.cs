using AutoMapper;
using BLL.DTO.Publisher;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class PublisherCatalogService : IPublisherCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public PublisherCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
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
            var publisher = _mapper.Map<Publisher>(item);

            publisher = await _repositoryWrapper.Publishers.AddAsync(publisher);

            await _repositoryWrapper.SaveChangesAsync();

            return publisher;
        }

        public async Task<Publisher> UpdateAsync(UpdatePublisherDto item)
        {
            var publisher = _mapper.Map<Publisher>(item);

            publisher = await _repositoryWrapper.Publishers.UpdateAsync(publisher.Id, publisher);

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

