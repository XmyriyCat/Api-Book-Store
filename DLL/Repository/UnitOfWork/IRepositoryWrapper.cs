using DLL.Repository.Interfaces;

namespace DLL.Repository.UnitOfWork
{
    public interface IRepositoryWrapper
    {
        IAuthorRepository Authors { get; }
        IBookRepository Books { get; }
        IDeliveryRepository Deliveries { get; }
        IGenreRepository Genres { get; }
        IOrderRepository Orders { get; }
        IOrderLineRepository OrderLines { get; }
        IPaymentWayRepository PaymentWays { get; }
        IPublisherRepository Publishers { get; }
        IRoleRepository Roles { get; }
        IShipmentRepository Shipments { get; }
        IUserRepository Users { get; }
        IWarehouseRepository Warehouses { get; }
        IWarehouseBookRepository WarehouseBooks { get; }

        void SaveChanges();
    }
}