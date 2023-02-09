using AutoMapper;
using BLL.DTO.Genre;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class GenreCatalogService : IGenreCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GenreCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _repositoryWrapper.Genres.GetAll().ToListAsync();
        }

        public async Task<Genre> FindAsync(int id)
        {
            return await _repositoryWrapper.Genres.FindAsync(id);
        }

        public async Task<Genre> AddAsync(CreateGenreDto item)
        {
            var genre = _mapper.Map<Genre>(item);

            genre = await _repositoryWrapper.Genres.AddAsync(genre);

            await _repositoryWrapper.SaveChangesAsync();

            return genre;
        }

        public async Task<Genre> UpdateAsync(UpdateGenreDto item)
        {
            var genre = _mapper.Map<Genre>(item);

            genre = await _repositoryWrapper.Genres.UpdateAsync(genre.Id, genre);

            await _repositoryWrapper.SaveChangesAsync();

            return genre;
        }

        public async Task DeleteAsync(int id)
        {
            await _repositoryWrapper.Genres.DeleteAsync(id);

            await _repositoryWrapper.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _repositoryWrapper.Genres.CountAsync();
        }
    }
}

