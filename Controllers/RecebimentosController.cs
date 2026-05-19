using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.Estoque;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("recebimentos")]
    [Authorize]
    public class RecebimentosController : ControllerBase
    {
        private readonly IRecebimentoService _service;

        public RecebimentosController(IRecebimentoService service)
        {
            _service = service;
        }

        [HttpGet("pedido/{numero:int}")]
        [Authorize(Policy = "recebimentos.read")]
        public async Task<IActionResult> GetPedidoByNumero(
            int numero,
            CancellationToken ct)
        {
            var pedido = await _service.GetPedidoByNumeroAsync(numero, ct);

            if (pedido is null)
                return NoContent();

            return Ok(pedido);
        }

        [HttpPost("lote")]
        [Authorize(Policy = "recebimentos.create")]
        public async Task<IActionResult> CriarLote(
    [FromBody] RecebimentoLoteCreateDto dto,
    CancellationToken ct)
        {
            try
            {
                var lote = await _service.CriarLoteAsync(dto, ct);
                return Ok(lote);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("pedido/{idPedidoCompra:int}/lotes")]
        [Authorize(Policy = "recebimentos.read")]
        public async Task<IActionResult> GetLotesByPedido(
    int idPedidoCompra,
    CancellationToken ct)
        {
            var lotes = await _service.GetLotesByPedidoAsync(idPedidoCompra, ct);
            return Ok(lotes);
        }
    }
}