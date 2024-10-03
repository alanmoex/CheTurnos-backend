using Application.Interfaces;
using Application.Models.Requests;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateAppointment ([FromBody] AppointmentCreateRequest request)
        {
            try
            {
                _appointmentService.CreateAppointment(request);
                return Ok();
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
    }
}
