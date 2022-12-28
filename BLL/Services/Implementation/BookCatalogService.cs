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

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _repositoryWrapper.Books.GetAllIncludeAsync();
        }

        public async Task<Book> FindAsync(int id)
        {
            return await _repositoryWrapper.Books.FindIncludeAsync(id);
        }

        public async Task<Book> AddAsync(CreateBookDto item)
        {
            var validationResult = await _createBookDtoValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("DTO is not valid");
            }

            var book = _mapper.Map<Book>(item);

            book.Authors = new List<Author>();
            book.Genres = new List<Genre>();

            foreach (var idAuthor in item.AuthorsId)
            {
                var author = await _repositoryWrapper.Authors.FindAsync(idAuthor);

                if (author is null)
                {
                    throw new ValidationException($"DTO contains a non-existent author id.");
                }

                book.Authors.Add(author);
            }

            foreach (var idGenre in item.GenresId)
            {
                var genre = await _repositoryWrapper.Genres.FindAsync(idGenre);

                if (genre is null)
                {
                    throw new ValidationException($"DTO contains a non-existent genre id.");
                }

                book.Genres.Add(genre);
            }

            var publisher = await _repositoryWrapper.Publishers.FindAsync(item.IdPublisher);

            if (publisher is null)
            {
                throw new ValidationException($"DTO contains a non-existent publisher id.");
            }
            
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

            book.Authors = new List<Author>();
            book.Genres = new List<Genre>();

            foreach (var idAuthor in item.AuthorsId)
            {
                var author = await _repositoryWrapper.Authors.FindAsync(idAuthor);
                
                if (author is null)
                {
                    throw new ValidationException($"DTO contains a non-existent author id.");
                }
                
                book.Authors.Add(author);
            }

            foreach (var idGenre in item.GenresId)
            {
                var genre = await _repositoryWrapper.Genres.FindAsync(idGenre);

                if (genre is null)
                {
                    throw new ValidationException($"DTO contains a non-existent genre id.");
                }

                book.Genres.Add(genre);
            }

            var publisher = await _repositoryWrapper.Publishers.FindAsync(item.IdPublisher);

            if (publisher is null)
            {
                throw new ValidationException($"DTO contains a non-existent publisher id.");
            }

            book.Publisher = publisher;

            await _repositoryWrapper.Books.UpdateAsync(book.Id, book);

            await _repositoryWrapper.SaveChangesAsync();

            return await _repositoryWrapper.Books.FindIncludeAsync(book.Id);
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

        private async Task<bool> IsExistingPublisherId(int publisherId)
        {
            var result = await _repositoryWrapper.Publishers.FindAsync(publisherId);

            return result is not null;
        }
    }
}

