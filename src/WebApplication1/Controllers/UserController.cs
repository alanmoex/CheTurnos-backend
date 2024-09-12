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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("[action]")]
        public ActionResult<List<UserDto?>> GetAllUsers()
        {
            return _userService.GetAllUsers();
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<UserDto?> GetUserById([FromRoute] int id)
        {
            try
            {
                return _userService.GetUserById(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<UserDto> CreateNewUser([FromBody] UserCreateRequest userCreateRequest)
        {
            return _userService.CreateNewUser(userCreateRequest);
        }

        [HttpPut("[action]/{id}")]
        public ActionResult ModifyUserData([FromRoute] int id, [FromBody] UserUpdateRequest userUpdateRequest)
        {
            try
            {
                _userService.ModifyUserData(id, userUpdateRequest);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}