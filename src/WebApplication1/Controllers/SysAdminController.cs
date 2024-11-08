using Application.Interfaces;
using Application.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysAdminController : ControllerBase
    {
        private readonly ISysAdminService _sysAdminService;

        public SysAdminController(ISysAdminService sysAdminService)
        {
            _sysAdminService = sysAdminService;
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
           return Ok( _sysAdminService.GetAll());
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById([FromRoute]int id) 
        { 
            return Ok(_sysAdminService.GetById(id));
        }

        [HttpPost("[action]")]
        public IActionResult Create([FromBody] SysAdminCreateRequestDTO dto)
        {
            _sysAdminService.Create(dto);
            return Ok();
        }

        [HttpPut("[action]/{idAdmin}")]
        public IActionResult Update([FromRoute] int idAdmin, [FromBody] SysAdminUpdateDTO dto)
        {
            _sysAdminService.Update(idAdmin, dto);
            return Ok();
        }

        [HttpPut("[Action]/{id}")]
        public IActionResult LogicalDelete([FromRoute] int id)
        {
            _sysAdminService.LogicalDelete(id);
            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _sysAdminService.Delete(id);
            return Ok();
        }


    }
}
