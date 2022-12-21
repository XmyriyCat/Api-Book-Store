using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Models
{
    public class Delivery
    {
        [Key]
        public int Id { get; set; }

        public decimal? Price { get; set; }

        public string? Name { get; set; }

        public virtual Shipment? Shipment { get; set; }
    }
}
