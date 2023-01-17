using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DLL.Models
{
    public class Role : IdentityRole<int>
    {
        [Key]
        public override int Id { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
