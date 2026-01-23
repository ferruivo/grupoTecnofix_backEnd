using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.PedidoCompra;
using GrupoTecnofix_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("pdf")]
    [Authorize]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService _pdfService;
        private readonly IPedidoCompraService _pedidoCompraService;
        private readonly IPedidoVendaService _pedidoVendaService;
        private readonly IEmpresaService _empresaService;
        private readonly IUsuariosService _usuariosService;
        private readonly ICurrentUserService _currentUser;


        public PdfController(IPdfService pdfService, IPedidoCompraService pedidoCompras, IPedidoVendaService pedidoVendas, IEmpresaService empresaService, IUsuariosService usuariosService, ICurrentUserService currentUser)
        {
            _pdfService = pdfService;
            _pedidoCompraService = pedidoCompras;
            _pedidoVendaService = pedidoVendas;
            _empresaService = empresaService;
            _usuariosService = usuariosService;
            _currentUser = currentUser;
        }

        [HttpPost("generate-base64")]
        public async Task<IActionResult> GenerateBase64([FromBody] object model)
        {
            if (model == null) return BadRequest("Modelo vazio.");

            var base64 = await _pdfService.GeneratePdfBase64Async(model);
            return Ok(new { base64 });
        }

        [Authorize(Policy = "pedidoscompra.read")]
        [HttpGet("pedidocompra/{id:int}")]
        public async Task<IActionResult> GeneratePedidoCompra(int id, CancellationToken ct)
        {
            var p = await _pedidoCompraService.GetByIdAsync(id, ct);
            if (p == null) return NotFound();

            var e = await _empresaService.GetAsync("",ct);
            var u = await _usuariosService.GetByIdAsync(_currentUser.GetUsuarioLogadoId(), ct);

            var base64 = await _pdfService.GeneratePurchaseOrderPdfBase64Async(e, u, p);

            return Ok(new { base64 });
        }

        [Authorize(Policy = "pedidosvenda.read")]
        [HttpGet("pedidovenda/{id:int}")]
        public async Task<IActionResult> GeneratePedidoVenda(int id, CancellationToken ct)
        {
            var p = await _pedidoVendaService.GetByIdAsync(id, ct);
            if (p == null) return NotFound();

            var e = await _empresaService.GetAsync("", ct);
            var u = await _usuariosService.GetByIdAsync(_currentUser.GetUsuarioLogadoId(), ct);

            var base64 = await _pdfService.GenerateSalesOrderPdfBase64Async(e, u, p);

            return Ok(new { base64 });
        }
    }
}
