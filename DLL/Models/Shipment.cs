using System.ComponentModel.DataAnnotations;

namespace DLL.Models
{
    public class Shipment
    {
        [Key]
        public int Id { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public int DeliveryId { get; set; }
        public virtual Delivery Delivery { get; set; }

        public int PaymentWayId { get; set; }
        public virtual PaymentWay PaymentWay { get; set; }

    }
}
