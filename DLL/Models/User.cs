using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DLL.Models
{
    public class User : IdentityUser<int>
    {
        [Key]
        public override int Id { get; set; }

        public string Login { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    }
}
