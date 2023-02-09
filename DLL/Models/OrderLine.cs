using System.ComponentModel.DataAnnotations;

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
