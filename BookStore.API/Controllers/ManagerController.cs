using BLL.DTO.Author;
using BLL.DTO.Delivery;
using BLL.DTO.Genre;
using BLL.DTO.PaymentWay;
using BLL.DTO.Publisher;
using BLL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBookStore.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ManagerController : ControllerBase
{
    private readonly IAuthorCatalogService _authorService;
    private readonly IGenreCatalogService _genreService;
    private readonly IPublisherCatalogService _publisherService;
    private readonly IPaymentWayCatalogService _paymentWayService;
    private readonly IDeliveryCatalogService _deliveryService;
    public ManagerController(IAuthorCatalogService authorService,
            IGenreCatalogService genreService,
            IPublisherCatalogService publisherService,
            IPaymentWayCatalogService paymentWayService,
            IDeliveryCatalogService deliveryService)
    {
        _authorService = authorService;
        _genreService = genreService;
        _publisherService = publisherService;
        _paymentWayService = paymentWayService;
        _deliveryService = deliveryService;
    }

    [AllowAnonymous]
    [HttpGet("author")]
    public async Task<IActionResult> AuthorGetAllAsyncTask()
    {
        var authors = await _authorService.GetAllAsync();
        return Ok(authors);
    }

    [AllowAnonymous]
    [HttpGet("genre")]
    public async Task<IActionResult> GenreGetAllAsyncTask()
    {
        var genres = await _genreService.GetAllAsync();
        return Ok(genres);
    }

    [AllowAnonymous]
    [HttpGet("publisher")]
    public async Task<IActionResult> PublisherGetAllAsyncTask()
    {
        var publishers = await _publisherService.GetAllAsync();
        return Ok(publishers);
    }

    [AllowAnonymous]
    [HttpGet("paymentWay")]
    public async Task<IActionResult> PaymentWayGetAllAsyncTask()
    {
        var paymentWays = await _paymentWayService.GetAllAsync();
        return Ok(paymentWays);
    }
    
    [AllowAnonymous]
    [HttpGet("delivery")]
    public async Task<IActionResult> DeliveryGetAllAsyncTask()
    {
        var deliveries = await _deliveryService.GetAllAsync();
        return Ok(deliveries);
    }

    [AllowAnonymous]
    [HttpGet("author/{id}")]
    public async Task<IActionResult> AuthorGetByIdAsyncTask(int id)
    {
        var author = await _authorService.FindAsync(id);

        if (author is null)
        {
            return NotFound();
        }

        return Ok(author);
    }

    [AllowAnonymous]
    [HttpGet("genre/{id}")]
    public async Task<IActionResult> GenreGetByIdAsyncTask(int id)
    {
        var genre = await _genreService.FindAsync(id);

        if (genre is null)
        {
            return NotFound();
        }

        return Ok(genre);
    }

    [AllowAnonymous]
    [HttpGet("publisher/{id}")]
    public async Task<IActionResult> PublisherGetByIdAsyncTask(int id)
    {
        var publisher = await _publisherService.FindAsync(id);

        if (publisher is null)
        {
            return NotFound();
        }

        return Ok(publisher);
    }

    [AllowAnonymous]
    [HttpGet("paymentWay/{id}")]
    public async Task<IActionResult> PaymentWayGetByIdAsyncTask(int id)
    {
        var paymentWay = await _paymentWayService.FindAsync(id);
    
        if (paymentWay is null)
        {
            return NotFound();
        }
    
        return Ok(paymentWay);
    }

    [AllowAnonymous]
    [HttpGet("delivery/{id}")]
    public async Task<IActionResult> DeliveryGetByIdAsyncTask(int id)
    {
        var delivery = await _deliveryService.FindAsync(id);
    
        if (delivery is null)
        {
            return NotFound();
        }
    
        return Ok(delivery);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("author")]
    public async Task<IActionResult> AuthorCreateAsyncTask([FromBody] CreateAuthorDto author)
    {
        if (author is null)
        {
            return BadRequest();
        }

        var createdAuthor = await _authorService.AddAsync(author);

        return CreatedAtAction(nameof(AuthorGetByIdAsyncTask), new { id = createdAuthor.Id }, createdAuthor);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("genre")]
    public async Task<IActionResult> GenreCreateAsyncTask([FromBody] CreateGenreDto genre)
    {
        if (genre is null)
        {
            return BadRequest();
        }

        var createGenre = await _genreService.AddAsync(genre);

        return CreatedAtAction(nameof(GenreGetByIdAsyncTask), new { id = createGenre.Id }, createGenre);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("publisher")]
    public async Task<IActionResult> PublisherCreateAsyncTask([FromBody] CreatePublisherDto publisher)
    {
        if (publisher is null)
        {
            return BadRequest();
        }

        var createPublisher = await _publisherService.AddAsync(publisher);

        return CreatedAtAction(nameof(PublisherCreateAsyncTask), new { id = createPublisher.Id }, createPublisher);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("paymentWay")]
    public async Task<IActionResult> PaymentWayCreateAsyncTask([FromBody] CreatePaymentWayDto paymentWay)
    {
        if (paymentWay is null)
        {
            return BadRequest();
        }
    
        var createPaymentWay = await _paymentWayService.AddAsync(paymentWay);
    
        return CreatedAtAction(nameof(PaymentWayCreateAsyncTask), new {id = createPaymentWay.Id}, createPaymentWay);
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPost("delivery")]
    public async Task<IActionResult> DeliveryCreateAsyncTask([FromBody] CreateDeliveryDto delivery)
    {
        if (delivery is null)
        {
            return BadRequest();
        }
    
        var createDelivery = await _deliveryService.AddAsync(delivery);
    
        return CreatedAtAction(nameof(DeliveryCreateAsyncTask), new {id = createDelivery.Id}, createDelivery);
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("author")]
    public async Task<IActionResult> AuthorUpdateAsyncTask([FromBody] UpdateAuthorDto author)
    {
        if (author is null)
        {
            return BadRequest();
        }

        var updatedAuthor = await _authorService.UpdateAsync(author);

        if (updatedAuthor is null)
        {
            return NotFound();
        }

        return Ok(updatedAuthor);
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("genre")]
    public async Task<IActionResult> GenreUpdateAsyncTask([FromBody] UpdateGenreDto genre)
    {
        if (genre is null)
        {
            return BadRequest();
        }

        var updateGenre = await _genreService.UpdateAsync(genre);

        if (updateGenre is null)
        {
            return NotFound();
        }

        return Ok(updateGenre);
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("publisher")]
    public async Task<IActionResult> PublisherUpdateAsyncTask([FromBody] UpdatePublisherDto publisher)
    {
        if (publisher is null)
        {
            return BadRequest();
        }

        var updatePublisher = await _publisherService.UpdateAsync(publisher);

        return Ok(updatePublisher);
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("paymentWay")]
    public async Task<IActionResult> PaymentWayUpdateAsyncTask([FromBody] UpdatePaymentWayDto paymentWay)
    {
        if (paymentWay is null)
        {
            return BadRequest();
        }
    
        var updatePaymentWay = await _paymentWayService.UpdateAsync(paymentWay);
    
        return Ok(updatePaymentWay);
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPut("delivery")]
    public async Task<IActionResult> DeliveryUpdateAsyncTask([FromBody] UpdateDeliveryDto delivery)
    {
        if (delivery is null)
        {
            return BadRequest();
        }
    
        var updateDelivery = await _deliveryService.UpdateAsync(delivery);
    
        return Ok(updateDelivery);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("author/{id}")]
    public async Task<IActionResult> AuthorDeleteAsyncTask(int id)
    {
        await _authorService.DeleteAsync(id);
        return Ok();
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("genre/{id}")]
    public async Task<IActionResult> GenreDeleteAsyncTask(int id)
    {
        await _genreService.DeleteAsync(id);
        return Ok();
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("publisher/{id}")]
    public async Task<IActionResult> PublisherDeleteAsyncTask(int id)
    {
        await _publisherService.DeleteAsync(id);
        return Ok();
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("paymentWay/{id}")]
    public async Task<IActionResult> PaymentWayDeleteAsyncTask(int id)
    {
        await _paymentWayService.DeleteAsync(id);
        return Ok();
    }
    
    [Authorize(Roles = "Manager")]
    [HttpDelete("delivery/{id}")]
    public async Task<IActionResult> DeliveryDeleteAsyncTask(int id)
    {
        await _deliveryService.DeleteAsync(id);
        return Ok();
    }
}