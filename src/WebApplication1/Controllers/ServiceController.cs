using Microsoft.AspNetCore.Mvc;
using Application.Models.Requests;
using Domain.Exceptions;
using Application;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServiceController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet("[action]")]
    public ActionResult<List<ServiceDTO>> GetAll()
    {
        return Ok(_serviceService.GetAll());
    }

    [HttpGet("[action]/{id}")]
    public ActionResult<ServiceDTO> GetById(int id)
    {
        try
        {
            return Ok(_serviceService.GetById(id));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet("[action]/{shopId}")]
    public ActionResult<List<ServiceDTO>> GetAllByShopId(int shopId)
    {
        try
        {
            return Ok(_serviceService.GetAllByShopId(shopId));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }


    [HttpGet("[action]/{shopId}")]
    public ActionResult<List<ShopsServicesByShopIdRequestDTO>> GetAllServicesByShopWithNameShop(int shopId)
    {
        return _serviceService.GetServicesOfShop(shopId);
    }

    [HttpPost("[action]")]
    public IActionResult Create([FromBody] ServiceCreateRequest serviceCreateRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var service = _serviceService.Create(serviceCreateRequest);
            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpDelete("[action]/{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _serviceService.Delete(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPut("[action]/{id}")]
    public IActionResult Update(int id, [FromBody] ServiceUpdateRequest serviceUpdateRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _serviceService.Update(id, serviceUpdateRequest);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }


}

