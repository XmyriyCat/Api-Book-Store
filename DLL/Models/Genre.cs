using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Models
{
    internal class Genre
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<BookGenre>? BookGenres { get; set; }
    }
}
