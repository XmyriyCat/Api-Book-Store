using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Classes
{
    public class ShipmentRepository : GenericRepository<Shipment>, IShipmentRepository
    {
        public ShipmentRepository(DbContext context) : base(context)
        {
        }
    }
}