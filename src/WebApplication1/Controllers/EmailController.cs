using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Models;
using Domain.Exceptions;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Application.Models.Requests;
using System.Security.Claims;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("[action]")]
        public IActionResult SendEmail ([FromBody]EmailDTO request)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "SysAdmin")
            {
                return Forbid();
            }
            try
            {
            _emailService.SendMail(request);
            return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
     
        [AllowAnonymous]
        [HttpPut("[action]")]
        public ActionResult RequestPasswordReset([FromBody] string email)
        {
            try
            {
                _emailService.RequestPassReset(email);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("[action]")]
        public ActionResult ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                _emailService.ResetPassword(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
