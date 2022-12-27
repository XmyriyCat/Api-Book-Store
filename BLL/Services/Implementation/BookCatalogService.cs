using System.Xml.XPath;
using AutoMapper;
using BLL.DTO.Book;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;

namespace BLL.Services.Implementation
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
            var validationResult = await _createBookDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }

            if (!await IsExistingAuthorsId(item.AuthorsId))
            {
                throw new ValidationException($"DTO contains a non-existent author id.");
            }

            if (!await IsExistingGenresId(item.GenresId))
            {
                throw new ValidationException($"DTO contains a non-existent genre id.");
            }

            var book = _mapper.Map<Book>(item);

            await _repositoryWrapper.Books.AddAsync(book);

            await _repositoryWrapper.SaveChangesAsync();

            return book;
        }

        public async Task<Book> UpdateAsync(UpdateBookDto item)
        {
            var validationResult = await _updateBookDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }

            var book = _mapper.Map<Book>(item);

            await _repositoryWrapper.Books.UpdateAsync(book);

            await _repositoryWrapper.SaveChangesAsync();

            return book;
        }

        public async Task DeleteAsync(int id)
        {
            await _repositoryWrapper.Books.DeleteAsync(id);

            await _repositoryWrapper.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _repositoryWrapper.Books.CountAsync();
        }

        private async Task<bool> IsExistingAuthorsId(IEnumerable<int> authorsId)
        {
            foreach (var id in authorsId)
            {
                if (!await IsExistingAuthorId(id))
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> IsExistingAuthorId(int authorId)
        {
            var result = await _repositoryWrapper.Authors.FindAsync(authorId);

            return result is not null;
        }

        private async Task<bool> IsExistingGenresId(IEnumerable<int> genresId)
        {
            foreach (var id in genresId)
            {
                if (!await IsExistingGenreId(id))
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> IsExistingGenreId(int genreId)
        {
            var result = await _repositoryWrapper.Genres.FindAsync(genreId);

            return result is not null;
        }
    }
}

