using AutoMapper;
using BLL.DTO.Author;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class AuthorCatalogService : IAuthorCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public AuthorCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _repositoryWrapper.Authors.GetAll().ToListAsync();
        }

        public async Task<Author> FindAsync(int id)
        {
            return await _repositoryWrapper.Authors.FindAsync(id);
        }

        public async Task<Author> AddAsync(CreateAuthorDto item)
        {
            var author = _mapper.Map<Author>(item);

            author = await _repositoryWrapper.Authors.AddAsync(author);

            await _repositoryWrapper.SaveChangesAsync();

            return author;
        }

        public async Task<Author> UpdateAsync(UpdateAuthorDto item)
        {
            var author = _mapper.Map<Author>(item);

            author = await _repositoryWrapper.Authors.UpdateAsync(author.Id, author);

            await _repositoryWrapper.SaveChangesAsync();

            return author;
        }

        public async Task DeleteAsync(int id)
        {
            await _repositoryWrapper.Authors.DeleteAsync(id);

            await _repositoryWrapper.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _repositoryWrapper.Authors.CountAsync();
        }
    }
}
