using AutoMapper;
using BLL.DTO.Author;
using BLL.Services.Interfaces;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using FluentValidation.Results;

namespace BLL.Services.Classes
{
    public class AuthorCatalogService : IAuthorCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAuthorDto> _createAuthorDtoValidator;
        private readonly IValidator<UpdateAuthorDto> _updateAuthorDtoValidator;

        public AuthorCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IValidator<CreateAuthorDto> createValidator, IValidator<UpdateAuthorDto> updateValidator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _createAuthorDtoValidator = createValidator;
            _updateAuthorDtoValidator = updateValidator;
        }

        public IEnumerable<Author> GetAll()
        {
            return _repositoryWrapper.Authors.GetAll();
        }

        public async Task<Author> FindAsync(int id)
        {
            return await _repositoryWrapper.Authors.FindAsync(id);
        }

        public async Task<Author> AddAsync(CreateAuthorDto item)
        {
            ValidationResult validationResult = await _createAuthorDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }
            
            Author author = _mapper.Map<Author>(item);

            await _repositoryWrapper.Authors.AddAsync(author);

            SaveChangesAsync();

            return author;
        }

        public async Task<Author> Update(UpdateAuthorDto item)
        {
            ValidationResult validationResult = await _updateAuthorDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }

            Author author = _mapper.Map<Author>(item);

            _repositoryWrapper.Authors.Update(author);

            SaveChangesAsync();

            return author;
        }
        public async Task DeleteAsync(int id)
        {
            await _repositoryWrapper.Authors.DeleteAsync(id);
            SaveChangesAsync();
        }

        public int Count()
        {
            return _repositoryWrapper.Authors.Count();
        }

        public async Task SaveChangesAsync()
        {
            await _repositoryWrapper.SaveChangesAsync();
        }
    }
}
