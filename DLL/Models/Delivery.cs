using System.ComponentModel.DataAnnotations;

namespace DLL.Models
{
    public class Delivery
    {
        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Shipment> Shipment { get; set; } = new List<Shipment>();
    }
}
