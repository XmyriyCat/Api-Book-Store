using BLL.DTO.PaymentWay;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentWayCatalogService _paymentService;

    public PaymentController(IPaymentWayCatalogService paymentService)
    {
        _paymentService = paymentService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> PaymentGetAllAsyncTask()
    {
        var deliveries = await _paymentService.GetAllAsync();
        return Ok(deliveries);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> PaymentGetByIdAsyncTask(int id)
    {
        var payment = await _paymentService.FindAsync(id);

        if (payment is null)
        {
            return NotFound();
        }

        return Ok(payment);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> PaymentCreateAsyncTask([FromBody] CreatePaymentWayDto payment)
    {
        if (payment is null)
        {
            return BadRequest();
        }

        var createdPayment = await _paymentService.AddAsync(payment);

        return new ObjectResult(createdPayment) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> PaymentUpdateAsyncTask([FromBody] UpdatePaymentWayDto payment)
    {
        if (payment is null)
        {
            return BadRequest();
        }

        var updatedPayment = await _paymentService.UpdateAsync(payment);

        if (updatedPayment is null)
        {
            return NotFound();
        }

        return Ok(updatedPayment);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> PaymentDeleteAsyncTask(int id)
    {
        await _paymentService.DeleteAsync(id);
        return Ok();
    }
}