using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllAppointment()
        {
            return Ok(_appointmentService.GetAllAppointment());
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAppointmentById([FromRoute] int id)
        {
            return Ok(_appointmentService.GetAppointmentById(id));
        }

        [HttpPost("[action]")]
        public IActionResult CreateAppointment([FromBody] AppointmentCreateRequest request)
        {
            try
            {
                _appointmentService.CreateAppointment(request.ShopId, request.ProviderId, request.DateAndHour, request.ServiceId, request.ClientId);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{employeeId}")]
        public ActionResult<List<Appointment>> GetAvailableAppointmentsByEmployeeId(int employeeId)
        {
            try
            {
                return Ok(_appointmentService.GetAvailableAppointmentsByEmployeeId(employeeId));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("[action]/{id}")]
        public IActionResult UpdateAppointment([FromBody] AppointmentUpdateRequest request, [FromRoute] int id)
        {
            try
            {
                _appointmentService.UpdateAppointment(request, id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteAppointment([FromRoute] int id)
        {
            try
            {
                _appointmentService.DeleteAppointment(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[action]")]
        public IActionResult AssignClient([FromBody] AssignClientRequestDTO request)
        {
            try
            {
                _appointmentService.AssignClient(request);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("[action]")]
        public ActionResult<List<Appointment?>> GetMyLastShopAppointment()
        {
            int ownerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            try
            {
                return _appointmentService.GetLastAppointmentByShopId(ownerId);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
        }
    }
}
