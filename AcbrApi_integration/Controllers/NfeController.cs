using AcbrApi_integration.Interfaces;
using AcbrApi_integration.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AcbrApi_integration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NfeController : ControllerBase
    {
        private readonly IAcbrNfeService _acbrNfeService;

        public NfeController(IAcbrNfeService acbrNfeService)
        {
            _acbrNfeService = acbrNfeService;
        }

        [HttpPost("{empresaKey}/previa/pdf")]
        public async Task<IActionResult> ObterPreviaPdf(string empresaKey,[FromBody] EmitirNfeRequest request,[FromQuery] bool logotipo = false,[FromQuery] bool nomeFantasia = false,[FromQuery] string formato = "padrao",[FromQuery] string mensagemRodape = "",[FromQuery] bool canhoto = true,CancellationToken cancellationToken = default)
        {
            var pdfBytes = await _acbrNfeService.ObterPreviaPdfAsync(
                empresaKey,
                request,
                logotipo,
                nomeFantasia,
                formato,
                mensagemRodape,
                canhoto,
                cancellationToken);

            return File(
                pdfBytes,
                "application/pdf",
                "previa-nfe.pdf");
        }

        [HttpPost("{empresaKey}/emitir")]
        public async Task<IActionResult> EmitirNfe(string empresaKey,[FromBody] EmitirNfeRequest request,CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Referencia))
            {
                request.Referencia = $"NFE-{DateTime.Now:yyyyMMddHHmmssfff}";
            }

            var response = await _acbrNfeService.EmitirNfeAsync(
                empresaKey,
                request,
                cancellationToken);

            return Ok(response);
        }

        [HttpGet("{empresaKey}/{idNfe}")]
        public async Task<IActionResult> ObterNfe(string empresaKey,string idNfe,CancellationToken cancellationToken)
        {
            var response = await _acbrNfeService.ObterNfeAsync(
                empresaKey,
                idNfe,
                cancellationToken);

            return Ok(response);
        }

        [HttpPost("{empresaKey}/listar")]
        public async Task<IActionResult> ListarNfe(string empresaKey, [FromBody] ListarNfeRequest request, CancellationToken cancellationToken)
        {
            var result = await _acbrNfeService.ListarNfeAsync(
                empresaKey,
                request,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{empresaKey}/{idNfe}/pdf")]
        public async Task<IActionResult> ObterPdf(string empresaKey,string idNfe,CancellationToken cancellationToken)
        {
            var pdfBytes = await _acbrNfeService.ObterPdfAsync(
                empresaKey,
                idNfe,
                logotipo: false,
                nomeFantasia: false,
                formato: "padrao",
                mensagemRodape: "",
                canhoto: true,
                cancellationToken: cancellationToken);

            return File(
                pdfBytes,
                "application/pdf",
                $"{idNfe}.pdf");
        }

        [HttpGet("{empresaKey}/{idNfe}/xml")]
        public async Task<IActionResult> ObterXml(string empresaKey,string idNfe,CancellationToken cancellationToken)
        {
            var xml = await _acbrNfeService.ObterXmlAsync(
                empresaKey,
                idNfe,
                cancellationToken);

            return Content(xml, "application/xml");
        }

        [HttpGet("{empresaKey}/{idNfe}/xml/download")]
        public async Task<IActionResult> DownloadXml(string empresaKey,string idNfe,CancellationToken cancellationToken)
        {
            var xml = await _acbrNfeService.ObterXmlAsync(
                empresaKey,
                idNfe,
                cancellationToken);

            var bytes = Encoding.UTF8.GetBytes(xml);

            return File(
                bytes,
                "application/xml",
                $"{idNfe}.xml");
        }

        [HttpPost("{empresaKey}/{idNfe}/carta-correcao")]
        public async Task<IActionResult> CartaCorrecao(string empresaKey,string idNfe,[FromBody] CartaCorrecaoRequest request,CancellationToken cancellationToken)
        {
            var response = await _acbrNfeService.CartaCorrecaoAsync(
                empresaKey,
                idNfe,
                request,
                cancellationToken);

            return Ok(response);
        }

        [HttpGet("{empresaKey}/{idNfe}/carta-correcao")]
        public async Task<IActionResult> ObterCartaCorrecao(string empresaKey,string idNfe,CancellationToken cancellationToken)
        {
            var response = await _acbrNfeService.ObterCartaCorrecaoAsync(
                empresaKey,
                idNfe,
                cancellationToken);

            return Ok(response);
        }

        [HttpGet("{empresaKey}/{idNfe}/carta-correcao/pdf")]
        public async Task<IActionResult> ObterCartaCorrecaoPdf(string empresaKey,string idNfe,CancellationToken cancellationToken)
        {
            var pdfBytes = await _acbrNfeService.ObterCartaCorrecaoPdfAsync(
                empresaKey,
                idNfe,
                cancellationToken);

            return File(
                pdfBytes,
                "application/pdf",
                $"{idNfe}-carta-correcao.pdf");
        }

        [HttpPost("{empresaKey}/{idNfe}/cancelamento")]
        public async Task<IActionResult> CancelarNfe(string empresaKey,string idNfe,[FromBody] CancelamentoNfeRequest request,CancellationToken cancellationToken)
        {
            var response = await _acbrNfeService.CancelarNfeAsync(
                empresaKey,
                idNfe,
                request,
                cancellationToken);

            return Ok(response);
        }

        [HttpGet("{empresaKey}/{idNfe}/cancelamento")]
        public async Task<IActionResult> ObterCancelamento(string empresaKey,string idNfe,CancellationToken cancellationToken)
        {
            var response = await _acbrNfeService.ObterCancelamentoAsync(
                empresaKey,
                idNfe,
                cancellationToken);

            return Ok(response);
        }

        [HttpGet("{empresaKey}/{idNfe}/cancelamento/pdf")]
        public async Task<IActionResult> ObterCancelamentoPdf(string empresaKey,string idNfe,CancellationToken cancellationToken)
        {
            var pdfBytes = await _acbrNfeService.ObterCancelamentoPdfAsync(
                empresaKey,
                idNfe,
                cancellationToken);

            return File(
                pdfBytes,
                "application/pdf",
                $"{idNfe}-cancelamento.pdf");
        }

        [HttpPost("{empresaKey}/eventos")]
        public async Task<IActionResult> ListarEventosNfe(string empresaKey,[FromBody] ListarEventosNfeRequest request,CancellationToken cancellationToken)
        {
            var response = await _acbrNfeService.ListarEventosNfeAsync(
                empresaKey,
                request,
                cancellationToken);

            return Ok(response);
        }
    }
}