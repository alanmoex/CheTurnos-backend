using Application.Interfaces;
using Application.Models.Requests;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;

        private readonly IConfiguration _config;

        public AuthenticationController(IAuthenticationService authenticationService, IConfiguration config)
        {
            _authenticationService = authenticationService;
            _config = config;
        }

        [HttpPost]
        public ActionResult<string> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            try
            {
            //Llama a un metodo que devuelve un string-Token
            string token = _authenticationService.Authenticate(authenticationRequest);
            return Ok(token);
            }
            catch (NotAllowedException ex)
            {
                // problemas del lado del cliente
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // problema de conectividad, etc.
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
