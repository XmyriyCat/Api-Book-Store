using System.ComponentModel.DataAnnotations;

namespace DLL.Models
{
    public class PaymentWay
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Shipment> Shipment { get; set; }
    }
}
