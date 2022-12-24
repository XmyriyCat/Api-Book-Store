namespace BLL.DTO.WarehouseBook;

public class UpdateWarehouseBookDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int WarehouseId { get; set; }

    public int BookId { get; set; }
}