using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Models
{
    internal class User
    {
        [Key]
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Login { get; set; }

        public int Password { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string? Email { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }

        public virtual ICollection<UserRole>? UserRoles { get; set; }

    }
}
