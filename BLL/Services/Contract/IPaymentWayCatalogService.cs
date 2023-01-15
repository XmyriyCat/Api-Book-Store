using BLL.DTO.PaymentWay;
using DLL.Models;

namespace BLL.Services.Contract;

public interface IPaymentWayCatalogService
{
    Task<IEnumerable<PaymentWay>> GetAllAsync();
    Task<PaymentWay> FindAsync(int id);
    Task<PaymentWay> AddAsync(CreatePaymentWayDto item);
    Task<PaymentWay> UpdateAsync(UpdatePaymentWayDto item);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}