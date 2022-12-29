using AutoMapper;
using BLL.DTO.Genre;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;

namespace BLL.Services.Implementation
{
    public class GenreCatalogService : IGenreCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateGenreDto> _createGenreDtoValidator;
        private readonly IValidator<UpdateGenreDto> _updateGenreDtoValidator;

        public GenreCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IValidator<CreateGenreDto> createValidator, IValidator<UpdateGenreDto> updateValidator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _createGenreDtoValidator = createValidator;
            _updateGenreDtoValidator = updateValidator;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _repositoryWrapper.Genres.GetAllIncludeAsync();
        }

        public async Task<Genre> FindAsync(int id)
        {
            return await _repositoryWrapper.Genres.FindIncludeAsync(id);
        }

        public async Task<Genre> AddAsync(CreateGenreDto item)
        {
            var validationResult = await _createGenreDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }

            var genre = _mapper.Map<Genre>(item);

            await _repositoryWrapper.Genres.AddAsync(genre);

            await _repositoryWrapper.SaveChangesAsync();

            return genre;
        }

        public async Task<Genre> UpdateAsync(UpdateGenreDto item)
        {
            var validationResult = await _updateGenreDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }

            var genre = _mapper.Map<Genre>(item);

            await _repositoryWrapper.Genres.UpdateAsync(genre.Id, genre);

            await _repositoryWrapper.SaveChangesAsync();

            return await _repositoryWrapper.Genres.FindIncludeAsync(genre.Id);
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

