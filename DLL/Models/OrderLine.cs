using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Models
{
    public class OrderLine
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int WarehouseBookId { get; set; }
        public virtual WarehouseBook WarehouseBook { get; set; }

    }
}
