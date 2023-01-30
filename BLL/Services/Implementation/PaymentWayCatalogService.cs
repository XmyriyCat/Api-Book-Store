using AutoMapper;
using BLL.DTO.PaymentWay;
using BLL.Services.Contract;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation;

public class PaymentWayCatalogService : IPaymentWayCatalogService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public PaymentWayCatalogService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<PaymentWay>> GetAllAsync()
    {
        return await _repositoryWrapper.PaymentWays.GetAll().ToListAsync();
    }

    public async Task<PaymentWay> FindAsync(int id)
    {
        return await _repositoryWrapper.PaymentWays.FindAsync(id);
    }
    
    public async Task<PaymentWay> AddAsync(CreatePaymentWayDto item)
    {
        var paymentWay = _mapper.Map<PaymentWay>(item);
    
        paymentWay = await _repositoryWrapper.PaymentWays.AddAsync(paymentWay);
    
        await _repositoryWrapper.SaveChangesAsync();
    
        return paymentWay;
    }
    
    public async Task<PaymentWay> UpdateAsync(UpdatePaymentWayDto item)
    {
        var paymentWay = _mapper.Map<PaymentWay>(item);
    
        paymentWay = await _repositoryWrapper.PaymentWays.UpdateAsync(paymentWay.Id, paymentWay);
    
        await _repositoryWrapper.SaveChangesAsync();
    
        return paymentWay;
    }
    
    public async Task DeleteAsync(int id)
    { 
        await _repositoryWrapper.PaymentWays.DeleteAsync(id);
    
        await _repositoryWrapper.SaveChangesAsync();
    }
    
    public async Task<int> CountAsync()
    {
        return await _repositoryWrapper.PaymentWays.CountAsync();
    }
}