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

        [AllowAnonymous]
        [HttpGet("[action]")]
        public ActionResult<List<OwnerDTO?>> GetAllOwners()
        {
            return _ownerService.GetAllOwners();
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<OwnerDTO?> GetOwnerById([FromRoute] int id)
        {
            try
            {
                return _ownerService.GetOwnerById(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public ActionResult<OwnerDTO> CreateNewOwner([FromBody] OwnerCreateRequest ownerCreateRequest)
        {
            try
            {
                return _ownerService.CreateNewOwner(ownerCreateRequest);
            }
            catch (ValidationException ex) //por si hay datos inválidos
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) //por si el email suministrado ya existe
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[action]")]
        public ActionResult ModifyOwnerData([FromBody] OwnerUpdateRequest ownerUpdateRequest)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            try
            {
                _ownerService.ModifyOwnerData(userId, ownerUpdateRequest);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("[action]/{id}")]
        public ActionResult PermanentDeletionOwner([FromRoute] int id)
        {
            try
            {
                _ownerService.PermanentDeletionOwner(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult LogicalDeletionOwner([FromRoute] int id)
        {
            try
            {
                _ownerService.LogicalDeletionOwner(id);
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
