using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public int ShipmentId { get; set; }
        public virtual Shipment Shipment { get; set; }

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<OrderLine> OrderLine { get; set; } = new List<OrderLine>();
    }
}
