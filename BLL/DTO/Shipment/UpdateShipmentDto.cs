namespace BLL.DTO.Shipment;

public class UpdateShipmentDto
{
    public int Id { get; set; }
    
    public int DeliveryId { get; set; }

    public int PaymentWayId { get; set; }
}