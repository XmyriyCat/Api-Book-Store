using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Models
{
    public class Shipment
    {
        [Key]
        public int Id { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }

        public int? DeliveryId { get; set; }
        public virtual Delivery? Delivery { get; set; }

        public int? PaymentWayId { get; set; }
        public virtual PaymentWay? PaymentWay { get; set; }

    }
}
