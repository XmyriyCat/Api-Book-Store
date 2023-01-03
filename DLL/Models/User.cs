using System.ComponentModel.DataAnnotations;

namespace DLL.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Login { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

    }
}
