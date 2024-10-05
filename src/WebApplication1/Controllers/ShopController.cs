using Microsoft.AspNetCore.Mvc;
using Application.Models.Requests; 
using Domain.Exceptions;
using Application;
using Application.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet("[action]")]
        public ActionResult<List<ShopDTO>> GetAll()
        {
            try
            {
                var shops = _shopService.GetAll();
                return Ok(shops);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<ShopDTO> GetById(int id)
        {
            try
            {
                var shop = _shopService.GetById(id);
                return Ok(shop);
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

        [HttpPost("[action]")]
        public IActionResult Create([FromBody] ShopCreateRequest shopCreateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var shop = _shopService.Create(shopCreateRequest);
                return CreatedAtAction(nameof(GetById), new { id = shop.Id }, shop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("[action]/{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] ShopUpdateRequest shopUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _shopService.Update(id, shopUpdateRequest);
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

        [HttpDelete("[action]/{id}")]
        public ActionResult PermanentDeletionShop([FromRoute] int id)
        {
            try
            {
                _shopService.PermanentDeletionShop(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult LogicalDeletionShop([FromRoute] int id)
        {
            try
            {
                _shopService.LogicalDeletionShop(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

