using System.ComponentModel.DataAnnotations;

namespace DLL.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }

        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

        public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

        public virtual ICollection<WarehouseBook> WarehouseBooks { get; set; } = new List<WarehouseBook>();
    }
}
