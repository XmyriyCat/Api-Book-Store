namespace BLL.DTO.Order;

public class CreateOrderDto
{
    public decimal TotalPrice { get; set; }

    public DateTime OrderDate { get; set; }

    public int ShipmentId { get; set; }
    
    public int CustomerId { get; set; }
}