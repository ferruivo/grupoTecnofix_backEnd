using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("clientes")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClientesService _service;

        public ClientesController(IClientesService service) => _service = service;

        [Authorize(Policy = "clientes.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
         => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "clientes.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

        [Authorize(Policy = "clientes.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteCreateUpdate dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "clientes.update")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteCreateUpdate dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }
    }
}
