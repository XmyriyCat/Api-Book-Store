namespace BLL.DTO.Warehouse_books;

public class UpdateWarehouseBooks
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int WarehouseId { get; set; }

    public int BookId { get; set; }
}