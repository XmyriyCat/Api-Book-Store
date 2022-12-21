using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Models
{
    public class WarehouseBook
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int? WarehouseId { get; set; }
        public virtual Warehouse? Warehouse { get; set; }

        public int? BookId { get; set; }
        public virtual Book? Book { get; set; }

        public virtual ICollection<OrderLine>? OrderLines { get; set; }
    }
}
