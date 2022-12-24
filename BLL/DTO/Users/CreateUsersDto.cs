using System.ComponentModel.DataAnnotations;
using DLL.Models;

namespace BLL.DTO.Users;

public class CreateUsersDto
{
    public string Username { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
    public string Email { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public virtual ICollection<int> OrderIds { get; set; }

    public virtual ICollection<int> RolesIds { get; set; }
}