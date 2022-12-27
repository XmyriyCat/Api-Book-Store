using DLL.Repository.Contract;
using DLL.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.UnitOfWork
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly DbContext _dbContext;
        private IAuthorRepository _author;
        private IBookRepository _book;
        private IDeliveryRepository _delivery;
        private IGenreRepository _genre;
        private IOrderRepository _order;
        private IOrderLineRepository _orderLine;
        private IPaymentWayRepository _paymentWay;
        private IPublisherRepository _publisher;
        private IRoleRepository _role;
        private IShipmentRepository _shipment;
        private IUserRepository _user;
        private IWarehouseRepository _warehouse;
        private IWarehouseBookRepository _warehouseBook;
        private bool _isDisposed;

        public RepositoryWrapper(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IAuthorRepository Authors
        {
            get
            {
                if (_author is null)
                {
                    _author = new AuthorRepository(_dbContext);
                }

                return _author;
            }
        }

        public IBookRepository Books
        {
            get
            {
                if (_book is null)
                {
                    _book = new BookRepository(_dbContext);
                }

                return _book;
            }
        }

        public IDeliveryRepository Deliveries
        {
            get
            {
                if (_delivery is null)
                {
                    _delivery = new DeliveryRepository(_dbContext);
                }

                return _delivery;
            }
        }

        public IGenreRepository Genres
        {
            get
            {
                if (_genre is null)
                {
                    _genre = new GenreRepository(_dbContext);
                }

                return _genre;
            }
        }

        public IOrderRepository Orders
        {
            get
            {
                if (_order is null)
                {
                    _order = new OrderRepository(_dbContext);
                }

                return _order;
            }
        }

        public IOrderLineRepository OrderLines
        {
            get
            {
                if (_orderLine is null)
                {
                    _orderLine = new OrderLineRepository(_dbContext);
                }

                return _orderLine;
            }
        }

        public IPaymentWayRepository PaymentWays
        {
            get
            {
                if (_paymentWay is null)
                {
                    _paymentWay = new PaymentWayRepository(_dbContext);
                }

                return _paymentWay;
            }
        }

        public IPublisherRepository Publishers
        {
            get
            {
                if (_publisher is null)
                {
                    _publisher = new PublisherRepository(_dbContext);
                }

                return _publisher;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                if (_role is null)
                {
                    _role = new RoleRepository(_dbContext);
                }

                return _role;
            }
        }

        public IShipmentRepository Shipments
        {
            get
            {
                if (_shipment is null)
                {
                    _shipment = new ShipmentRepository(_dbContext);
                }

                return _shipment;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (_user is null)
                {
                    _user = new UserRepository(_dbContext);
                }

                return _user;
            }
        }

        public IWarehouseRepository Warehouses
        {
            get
            {
                if (_warehouse is null)
                {
                    _warehouse = new WarehouseRepository(_dbContext);
                }

                return _warehouse;
            }
        }

        public IWarehouseBookRepository WarehouseBooks
        {
            get
            {
                if (_warehouseBook is null)
                {
                    _warehouseBook = new WarehouseBookRepository(_dbContext);
                }

                return _warehouseBook;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                _isDisposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}