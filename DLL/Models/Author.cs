using System.ComponentModel.DataAnnotations;

namespace DLL.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set;}

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
