using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

    public class OwnerController : Controller
    {
        private readonly IOwnerService _ownerService;

        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        [HttpPut("[action]")]
        public ActionResult AddNewAppointments([FromBody] AddNewAppointmentsRequest addNewAppointmentsRequest)
        {
            int ownerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            try
            {
                _ownerService.AddNewAppointments(ownerId, addNewAppointmentsRequest.DateStart, addNewAppointmentsRequest.DateEnd);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
