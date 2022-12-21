using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Models
{
    internal class Warehouse
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        [RegularExpression(@"^\+375 \((17 | 29 | 44)\) [0-9]{3}-[0-9]{2}-[0 - 9]{ 2}$")]
        public string? PhoneNumber { get; set; }

        public virtual ICollection<WarehouseBook>? WarehouseBooks { get; set; }
    }
}
