using BLL.DTO.Publisher;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublisherController : ControllerBase
{
    private readonly IPublisherCatalogService _publisherService;

    public PublisherController(IPublisherCatalogService publisherService)
    {
        _publisherService = publisherService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> PublisherGetAllAsyncTask()
    {
        var deliveries = await _publisherService.GetAllAsync();
        return Ok(deliveries);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> PublisherGetByIdAsyncTask(int id)
    {
        var publisher = await _publisherService.FindAsync(id);

        if (publisher is null)
        {
            return NotFound();
        }

        return Ok(publisher);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost]
    public async Task<IActionResult> PublisherCreateAsyncTask([FromBody] CreatePublisherDto publisher)
    {
        if (publisher is null)
        {
            return BadRequest();
        }

        var createdPublisher = await _publisherService.AddAsync(publisher);

        return new ObjectResult(createdPublisher) { StatusCode = StatusCodes.Status201Created };
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> PublisherUpdateAsyncTask([FromBody] UpdatePublisherDto publisher)
    {
        if (publisher is null)
        {
            return BadRequest();
        }

        var updatedPublisher = await _publisherService.UpdateAsync(publisher);

        if (updatedPublisher is null)
        {
            return NotFound();
        }

        return Ok(updatedPublisher);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> PublisherDeleteAsyncTask(int id)
    {
        await _publisherService.DeleteAsync(id);
        return Ok();
    }
}