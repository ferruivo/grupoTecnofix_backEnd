using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.Estoque;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api")]
    public class LiberacaoPedidosController : ControllerBase
    {
        private readonly ILiberacaoPedidoService _service;

        public LiberacaoPedidosController(ILiberacaoPedidoService service)
        {
            _service = service;
        }

        [HttpGet("pendentesliberacao")]
        [Authorize(Policy = "liberacaopedidos.read")]
        public async Task<IActionResult> GetPendentesLiberacao([FromQuery] int idCliente, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            var result = await _service.GetPendentesLiberacaoAsync(idCliente, page, pageSize, ct);
            return Ok(result);
        }

        [HttpGet("lotes/disponiveis")]
        [Authorize(Policy = "liberacaopedidos.create")]
        public async Task<IActionResult> GetLotesDisponiveis([FromQuery] int idProduto, CancellationToken ct)
        {
            var result = await _service.GetLotesDisponiveisAsync(idProduto, ct);
            return Ok(result);
        }

        [HttpPost("liberar")]
        [Authorize(Policy = "liberacaopedidos.create")]
        public async Task<IActionResult> Liberar([FromBody] LiberarPendenteRequestDto dto, CancellationToken ct)
        {
            var result = await _service.LiberarPendenteAsync(dto, ct);
            return Ok(result);
        }

        [HttpGet("liberacao/alocacoes")]
        [Authorize(Policy = "liberacaopedidos.create")]
        public async Task<IActionResult> GetAlocacoes([FromQuery] int idPedidoVenda, [FromQuery] int item, CancellationToken ct)
        {
            var result = await _service.GetAlocacoesLiberadasAsync(idPedidoVenda, item, ct);

            if (result is null)
                return NotFound(new { message = "Item do pedido não encontrado." });

            return Ok(result);
        }

        [HttpPost("estornar")]
        [Authorize(Policy = "liberacaopedidos.create")]
        public async Task<IActionResult> Estornar([FromBody] EstornarLiberacaoRequestDto dto, CancellationToken ct)
        {
            var result = await _service.EstornarLiberacaoAsync(dto, ct);
            return Ok(result);
        }

        [HttpGet("liberacao/alocacoesOrdemExpedicao")]
        [Authorize(Policy = "liberacaopedidos.create")]
        public async Task<IActionResult> GetAlocacoesOrdemExpedicao([FromQuery] int idCliente, CancellationToken ct)
        {
            var result = await _service.GetExpedicaoConsumoLoteAsync(idCliente, ct);

            if (result is null)
                return NotFound(new { message = "Nenhum item liberado para o cliente." });

            return Ok(result);
        }
    }
}
