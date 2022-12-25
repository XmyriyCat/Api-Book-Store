using AutoMapper;
using BLL.DTO.Book;
using BLL.Services.Interfaces;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using FluentValidation.Results;

namespace BLL.Services.Classes
{
    public class BookCatalogService : IBookCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookDto> _createBookDtoValidator;
        private readonly IValidator<UpdateBookDto> _updateBookDtoValidator;

        public BookCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IValidator<CreateBookDto> createValidator, IValidator<UpdateBookDto> updateValidator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _createBookDtoValidator = createValidator;
            _updateBookDtoValidator = updateValidator;
        }

        public IEnumerable<Book> GetAll()
        {
            return _repositoryWrapper.Books.GetAll();
        }

        public async Task<Book> FindAsync(int id)
        {
            return await _repositoryWrapper.Books.FindAsync(id);
        }

        public async Task<Book> AddAsync(CreateBookDto item)
        {
            ValidationResult validationResult = await _createBookDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }

            Book book = _mapper.Map<Book>(item);

            await _repositoryWrapper.Books.AddAsync(book);

            SaveChangesAsync();

            return book;
        }

        public async Task<Book> Update(UpdateBookDto item)
        {
            ValidationResult validationResult = await _updateBookDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }

            Book book = _mapper.Map<Book>(item);

            _repositoryWrapper.Books.Update(book);

            SaveChangesAsync();

            return book;
        }
        public async Task DeleteAsync(int id)
        {
            await _repositoryWrapper.Books.DeleteAsync(id);
            SaveChangesAsync();
        }

        public int Count()
        {
            return _repositoryWrapper.Books.Count();
        }

        public async Task SaveChangesAsync()
        {
            await _repositoryWrapper.SaveChangesAsync();
        }
    }
}

