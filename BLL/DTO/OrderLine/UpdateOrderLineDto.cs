namespace BLL.DTO.OrderLine;

public class UpdateOrderLineDto
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int OrderId { get; set; }

    public int WarehouseBookId { get; set; }
}