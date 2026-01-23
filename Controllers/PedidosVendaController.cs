using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.PedidoVenda;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("pedidosvenda")]
    [Authorize]
    public class PedidosVendaController : ControllerBase
    {
        private readonly IPedidoVendaService _service;

        public PedidosVendaController(IPedidoVendaService service) => _service = service;

        [Authorize(Policy = "pedidosvenda.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
            => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "pedidosvenda.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => Ok(await _service.GetByIdAsync(id, ct));

        [Authorize(Policy = "pedidosvenda.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoVendaCreateUpdateDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [Authorize(Policy = "pedidosvenda.update")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PedidoVendaCreateUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [Authorize(Policy = "pedidosvenda.delete")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}