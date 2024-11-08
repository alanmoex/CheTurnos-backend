using Application;
using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Application.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [HttpGet ("[action]")]
        public ActionResult<List<EmployeeResponseDTO>> GetAll()
        {
            return Ok(_employeeService.GetAll());
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_employeeService.GetById(id));
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
        public ActionResult<List<EmployeeResponseDTO>> GetAvailables(int shopId)
        {
            try
            {
                return Ok(_employeeService.GetAvailables(shopId));
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
        public ActionResult<List<EmployeeResponseDTO>> GetAllByShopId(int shopId)
        {
            try
            {
                return Ok(_employeeService.GetAllByShopId(shopId));
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
        public IActionResult Create([FromBody] EmployeeCreateRequestDTO request)
        {
            try
            {
                var service = _employeeService.Create(request);
                return Ok("Se creo correctamente");
            }
            /*catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }*/
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
                _employeeService.Delete(id);
                return Ok("Se elimino correctamente");
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

        [HttpPut("[action]")]
        public IActionResult Update([FromBody] EmployeeUpdateRequest request)
        {

            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            try
            {
                _employeeService.Update(userId, request);
                return Ok("Se actualizo con exito");
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

        [HttpGet("[action]/{ownerId}")]
        public ActionResult<List<EmployeeResponseDTO?>> GetMyShopEmployees([FromRoute] int ownerId)
        {
            //int ownerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            return _employeeService.GetMyShopEmployees(ownerId);
        }

    }
}
