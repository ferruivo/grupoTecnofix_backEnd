using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.PedidoCompra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("pedido-compra")]
    [Authorize]
    public class PedidoCompraController : ControllerBase
    {
        private readonly IPedidoCompraService _service;

        public PedidoCompraController(IPedidoCompraService service) => _service = service;

        //[Authorize(Policy = "pedidocompra.read")]
        //[HttpGet]
        //public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
        //    => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        //[Authorize(Policy = "pedidocompra.read")]
        //[HttpGet("{id:int}")]
        //public async Task<IActionResult> GetById(int id, CancellationToken ct)
        //    => Ok(await _service.GetByIdAsync(id, ct));

        //[Authorize(Policy = "pedidocompra.create")]
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] PedidoCompraCreateUpdateDto dto, CancellationToken ct)
        //{
        //    var id = await _service.CreateAsync(dto, ct);
        //    return CreatedAtAction(nameof(GetById), new { id }, new { id });
        //}

        //[Authorize(Policy = "pedidocompra.update")]
        //[HttpPut("{id:int}")]
        //public async Task<IActionResult> Update(int id, [FromBody] PedidoCompraCreateUpdateDto dto, CancellationToken ct)
        //{
        //    await _service.UpdateAsync(id, dto, ct);
        //    return NoContent();
        //}

        //[Authorize(Policy = "pedidocompra.delete")]
        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> Delete(int id, CancellationToken ct)
        //{
        //    await _service.DeleteAsync(id, ct);
        //    return NoContent();
        //}
    }
}
