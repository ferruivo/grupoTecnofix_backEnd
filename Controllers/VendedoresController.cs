
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("vendedores")]
    [Authorize]
    public class VendedoresController : ControllerBase
    {
        private readonly IVendedoresService _service;

        public VendedoresController(IVendedoresService service) => _service = service;

        [Authorize(Policy = "vendedores.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "vendedores.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

        [Authorize(Policy = "vendedores.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VendedorCreateDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "vendedores.update")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VendedorUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }
    }
}
