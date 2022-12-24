namespace BLL.DTO.OrderLine;

public class CreateOrderLineDto
{
    public int Quantity { get; set; }

    public int OrderId { get; set; }

    public int WarehouseBookId { get; set; }
}