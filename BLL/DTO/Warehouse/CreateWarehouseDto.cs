using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Warehouse;

public class CreateWarehouseDto
{
    public string Name { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    [RegularExpression(@"^\+375 \((17 | 29 | 33 | 44)\) [0-9]{3}-[0-9]{2}-[0 - 9]{ 2}$")]
    public string PhoneNumber { get; set; }
}