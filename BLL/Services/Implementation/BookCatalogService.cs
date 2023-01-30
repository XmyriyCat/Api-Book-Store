using AutoMapper;
using BLL.DTO.Book;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;

namespace BLL.Services.Implementation
{
    public class BookCatalogService : IBookCatalogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public BookCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
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
            var book = _mapper.Map<Book>(item);

            book.Authors = new HashSet<Author>();
            book.Genres = new HashSet<Genre>();

            foreach (var idAuthor in item.AuthorsId)
            {
                var author = await _repositoryWrapper.Authors.FindAsync(idAuthor);
                
                book.Authors.Add(author);
            }

            foreach (var idGenre in item.GenresId)
            {
                var genre = await _repositoryWrapper.Genres.FindAsync(idGenre);
                
                book.Genres.Add(genre);
            }

            var publisher = await _repositoryWrapper.Publishers.FindAsync(item.IdPublisher);
            
            book.Publisher = publisher;

            book = await _repositoryWrapper.Books.AddAsync(book);

            await _repositoryWrapper.SaveChangesAsync();

            return book;
        }

        public async Task<Book> UpdateAsync(UpdateBookDto item)
        {
            var book = _mapper.Map<Book>(item);

            book.Authors = new List<Author>();
            book.Genres = new List<Genre>();

            foreach (var idAuthor in item.AuthorsId)
            {
                var author = await _repositoryWrapper.Authors.FindAsync(idAuthor);
                book.Authors.Add(author);
            }

            foreach (var idGenre in item.GenresId)
            {
                var genre = await _repositoryWrapper.Genres.FindAsync(idGenre);
                book.Genres.Add(genre);
            }

            var publisher = await _repositoryWrapper.Publishers.FindAsync(item.IdPublisher);
            
            book.Publisher = publisher;

            book = await _repositoryWrapper.Books.UpdateAsync(book.Id, book);

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
    }
}

