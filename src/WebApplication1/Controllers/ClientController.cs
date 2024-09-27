using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("[action]")]
        public ActionResult<List<ClientDto?>> GetAllClients()
        {
            return _clientService.GetAllClients();
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<ClientDto?> GetClientById([FromRoute] int id)
        {
            try
            {
                return _clientService.GetClientById(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<ClientDto> CreateNewClient([FromBody] ClientCreateRequest clientCreateRequest)
        {
            return _clientService.CreateNewClient(clientCreateRequest);
        }

        [HttpPut("[action]/{id}")]
        public ActionResult ModifyClientData([FromRoute] int id, [FromBody] ClientUpdateRequest clientUpdateRequest)
        {
            try
            {
                _clientService.ModifyClientData(id, clientUpdateRequest);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult DeleteClient([FromRoute] int id)
        {
            try
            {
                _clientService.DeleteClient(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}