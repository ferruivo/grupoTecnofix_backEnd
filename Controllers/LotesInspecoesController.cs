using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("lotes/{idLote:int}/inspecoes")]
    [Authorize]
    public class LotesInspecoesController : ControllerBase
    {
        private readonly ILotesInspecoesService _service;
        private readonly AppDbContext _db;

        public LotesInspecoesController(ILotesInspecoesService service, AppDbContext db)
        {
            _service = service;
            _db = db;
        }

        [Authorize(Policy = "inspecaolote.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] int idLote, CancellationToken ct = default)
            => Ok(await _service.GetByLoteAsync(idLote, ct));

        [Authorize(Policy = "inspecaolote.read")]
        [HttpGet("{idEspecificacao:int}")]
        public async Task<IActionResult> GetByEspecificacao([FromRoute] int idLote, [FromRoute] int idEspecificacao, CancellationToken ct = default)
            => Ok(await _service.GetByLoteEspecificacaoAsync(idLote, idEspecificacao, ct));

        [Authorize(Policy = "inspecaolote.update")]
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBatch([FromRoute] int idLote, [FromBody] LoteInspecaoBatchDto dto, CancellationToken ct = default)
        {
            // Ensure every item uses the route lote id
            foreach (var it in dto.Items)
                it.IdLote = idLote;

            // create or update all inspecoes
            await _service.CreateOrUpdateBatchAsync(idLote, dto.Items, dto.StatusLote, ct);

            // If a lote status was provided, update the lote status
            if (!string.IsNullOrWhiteSpace(dto.StatusLote))
            {
                var lote = await _db.Lotes.FirstOrDefaultAsync(l => l.IdLote == idLote, ct);
                if (lote == null)
                    return NotFound("Lote não encontrado.");

                lote.Status = dto.StatusLote;
                await _db.SaveChangesAsync(ct);
            }

            return NoContent();
        }

        [Authorize(Policy = "inspecaolote.update")]
        [HttpDelete("{idLoteInspecao:int}")]
        public async Task<IActionResult> Delete([FromRoute] int idLote, [FromRoute] int idLoteInspecao, CancellationToken ct = default)
        {
            await _service.DeleteAsync(idLoteInspecao, ct);
            return NoContent();
        }

        // GET /lotes/pendentes - retorna todos os lotes com status 'I'
        [Authorize(Policy = "inspecaolote.read")]
        [HttpGet("/lotes/pendentes")]
        public async Task<IActionResult> GetLotesPendentes(CancellationToken ct = default)
        {
            var items = await _db.Lotes
                .AsNoTracking()
                .Where(l => l.Status == "I")
                .OrderBy(l => l.IdLote)
                .Select(l => new RecebimentoLoteDto
                {
                    Id = l.IdLote,
                    IdLote = l.IdLote,

                    IdPedidoCompra = l.PedidoCompra,
                    IdPedidoCompraItem = l.ItemPedidoCompra,

                    Item = l.ItemPedidoCompra,
                    
                    IdProduto = l.IdProduto,
                    ProdutoCodigo = _db.Produtos
                        .Where(p => p.IdProduto == l.IdProduto)
                        .Select(p => p.Codigo)
                        .FirstOrDefault(),

                    ProdutoDescricao = _db.Produtos
                        .Where(p => p.IdProduto == l.IdProduto)
                        .Select(p => p.Descricao)
                        .FirstOrDefault(),

                    Data = l.DataEntrada,

                    Nf = l.NfCompra == 0 ? null : l.NfCompra.ToString(),
                    Certificado = l.Certificado,

                    QuantidadeRecebida = l.Quantidade,
                    Status = l.Status,
                    Observacao = l.Obs
                })
                .ToListAsync(ct);

            return Ok(items);
        }
    }
}
