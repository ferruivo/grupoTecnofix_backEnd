
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("empresa")]
    [Authorize] 
    public class EmpresaController : ControllerBase
    {

        private readonly IEmpresaService _service;

        public EmpresaController(IEmpresaService service) => _service = service;
        

        [Authorize(Policy = "empresa.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetAsync(search, ct));

        [Authorize(Policy = "empresa.update")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmpresaUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }
    }
}

