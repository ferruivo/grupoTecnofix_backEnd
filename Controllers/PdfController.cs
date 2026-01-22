using Microsoft.AspNetCore.Mvc;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.PedidoCompra;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService _pdfService;

        public PdfController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpPost("generate-base64")]
        public async Task<IActionResult> GenerateBase64([FromBody] object model)
        {
            if (model == null) return BadRequest("Modelo vazio.");

            var base64 = await _pdfService.GeneratePdfBase64Async(model);
            return Ok(new { base64 });
        }

        [HttpPost("pedido-compra/generate-base64")]
        public async Task<IActionResult> GeneratePedidoCompra([FromBody] PedidoCompraExportDto model)
        {
            if (model == null) return BadRequest("Modelo vazio.");

            var base64 = await _pdfService.GeneratePurchaseOrderPdfBase64Async(new PedidoCompraDto
            {
                IdPedidoCompra = model.IdPedidoCompra,
                DataPedido = model.DataPedido,
                FornecedorNome = model.FornecedorNome,
                ValorFrete = model.ValorFrete,
                TotalProdutos = model.TotalProdutos,
                TotalIpi = model.TotalIpi,
                TotalIcms = model.TotalIcms,
                TotalPedido = model.TotalPedido,
                Observacao = model.Observacao,
                Itens = model.Itens.Select(i => new PedidoCompraItemDto
                {
                    ProdutoCodigo = i.ProdutoCodigo,
                    ProdutoDescricao = i.ProdutoDescricao,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    TotalItem = i.TotalItem
                }).ToList()
            });

            return Ok(new { base64 });
        }
    }
}
