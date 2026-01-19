using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("transportadoras")]
    [Authorize]
    public class TransportadorasController : ControllerBase
    {
        private readonly ITransportadorasService _service;

        public TransportadorasController(ITransportadorasService service) => _service = service;

        [Authorize(Policy = "transportadoras.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "transportadoras.read")]
        [HttpGet("lookup")]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetListAsync(search, ct));

        [Authorize(Policy = "transportadoras.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

        [Authorize(Policy = "transportadoras.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransportadoraCreateDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "transportadoras.update")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TransportadoraUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [Authorize(Policy = "transportadoras.read")]
        [HttpPost("export")]
        public async Task<IActionResult> Export([FromQuery] string? search = null, CancellationToken ct = default)
        {
            var bytes = await _service.ExportListToExcelAsync(search, ct);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "transportadoras.xlsx");
        }
    }
}
