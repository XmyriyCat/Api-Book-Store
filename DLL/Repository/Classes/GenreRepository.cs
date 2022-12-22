using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;

namespace DLL.Repository.Classes
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(ShopDbContext context) : base(context)
        {
        }
    }
}