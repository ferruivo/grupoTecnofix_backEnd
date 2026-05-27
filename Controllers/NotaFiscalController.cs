using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.NotaFiscal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("api/notas-fiscais")]
    [Authorize]
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalService _service;
        private readonly INotaFiscalPreviewService _previewService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public NotaFiscalController(INotaFiscalService service, INotaFiscalPreviewService previewService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _service = service;
            _previewService = previewService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Policy = "notafiscal.read")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            var result = await _service.GetPagedAsync(page, pageSize, search, ct);
            return Ok(result);
        }

        [HttpGet("{idNotaFiscal:long}")]
        [Authorize(Policy = "notafiscal.read")]
        public async Task<IActionResult> GetById(
            long idNotaFiscal,
            CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(idNotaFiscal, ct);
            return Ok(result);
        }

        [HttpPost("preview")]
        public async Task<IActionResult> GerarPreview([FromBody] NotaFiscalPreviewRequestDto dto,CancellationToken ct)
        {
            var result = await _previewService.GerarPreviewAsync(dto, ct);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "notafiscal.create")]
        public async Task<IActionResult> Create([FromBody] NotaFiscalCreateDto dto,CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);

            return Ok(new
            {
                idNotaFiscal = id,
                message = "Nota fiscal criada com sucesso."
            });
        }

        [HttpPut("{idNotaFiscal:long}")]
        [Authorize(Policy = "notafiscal.update")]
        public async Task<IActionResult> Update(long idNotaFiscal,[FromBody] NotaFiscalUpdateDto dto,CancellationToken ct)
        {
            await _service.UpdateAsync(idNotaFiscal, dto, ct);

            return Ok(new
            {
                message = "Nota fiscal atualizada com sucesso."
            });
        }

        [HttpDelete("{idNotaFiscal:long}")]
        [Authorize(Policy = "notafiscal.delete")]
        public async Task<IActionResult> Delete(long idNotaFiscal,CancellationToken ct)
        {
            await _service.DeleteAsync(idNotaFiscal, ct);

            return Ok(new
            {
                message = "Nota fiscal excluída com sucesso."
            });
        }

        [HttpPost("{idNotaFiscal:long}/eventos")]
        [Authorize(Policy = "notafiscal.create")]
        public async Task<IActionResult> AddEvento(long idNotaFiscal,[FromBody] NotaFiscalEventoCreateDto dto,CancellationToken ct)
        {
            dto.IdNotaFiscal = idNotaFiscal;

            await _service.AddEventoAsync(dto, ct);

            return Ok(new
            {
                message = "Evento registrado com sucesso."
            });
        }

        [HttpGet("{idNotaFiscal:long}/eventos")]
        [Authorize(Policy = "notafiscal.read")]
        public async Task<IActionResult> GetEventos(long idNotaFiscal,CancellationToken ct)
        {
            var eventos = await _service.GetEventosAsync(idNotaFiscal, ct);
            return Ok(eventos);
        }

        [HttpPost("{idNotaFiscal:long}/emitir")]
        [Authorize(Policy = "notafiscal.create")]
        public async Task<IActionResult> Emitir(long idNotaFiscal,CancellationToken ct)
        {
            await _service.EmitirAsync(idNotaFiscal, ct);

            var nota = await _service.GetByIdAsync(idNotaFiscal, ct);

            var status = nota?.Status ?? string.Empty;

            string message = status.ToUpperInvariant() switch
            {
                "AUTORIZADA" => "Nota fiscal Autorizada",
                "REJEITADA" => "Nota fiscal rejeitada",
                "CANCELADA" => "Nota fiscal Cancelada",
                _ => "Nota fiscal enviada para emissão."
            };

            return Ok(new { message, status });
        }

        [HttpPost("importar-itens-preview")]
        [Authorize(Policy = "notafiscal.create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportarItensPreview([FromForm] NotaFiscalImportarItensFormDto form,CancellationToken ct)
        {
            var result = await _service.ImportarItensPreviewAsync(form.Arquivo, ct);
            return Ok(result);
        }
        
        [HttpGet("{idNotaFiscal:long}/xml")]
        [Authorize(Policy = "notafiscal.read")]
        public async Task<IActionResult> ObterXml(long idNotaFiscal, CancellationToken ct)
        {
            var nota = await _service.GetByIdAsync(idNotaFiscal, ct);
            if (nota == null || string.IsNullOrEmpty(nota.IdNfe))
                return NotFound("Nota fiscal não encontrada ou sem IdNfe.");

            var empresaKey = _configuration.GetSection("AcbrApi")["EmpresaKey"] ?? string.Empty;

            var client = _httpClientFactory.CreateClient("AcbrApi");
            var xml = await client.GetStringAsync($"api/nfe/{empresaKey}/{nota.IdNfe}/xml", ct);
            return Content(xml, "application/xml");
        }

        [HttpGet("{idNotaFiscal:long}/danfe")]
        [Authorize(Policy = "notafiscal.read")]
        public async Task<IActionResult> ObterDanfe(long idNotaFiscal, CancellationToken ct)
        {
            var nota = await _service.GetByIdAsync(idNotaFiscal, ct);
            if (nota == null || string.IsNullOrEmpty(nota.IdNfe))
                return NotFound("Nota fiscal não encontrada ou sem IdNfe.");

            var empresaKey = _configuration.GetSection("AcbrApi")["EmpresaKey"] ?? string.Empty;

            var client = _httpClientFactory.CreateClient("AcbrApi");
            var pdfBytes = await client.GetByteArrayAsync($"api/nfe/{empresaKey}/{nota.IdNfe}/pdf", ct);
            return File(pdfBytes, "application/pdf", $"{nota.IdNfe}.pdf");
        }

        [HttpGet("{idNotaFiscal:long}/danfeCancelado")]
        [Authorize(Policy = "notafiscal.read")]
        public async Task<IActionResult> ObterDanfeCancelado(long idNotaFiscal, CancellationToken ct)
        {
            var nota = await _service.GetByIdAsync(idNotaFiscal, ct);
            if (nota == null || string.IsNullOrEmpty(nota.IdNfe))
                return NotFound("Nota fiscal não encontrada ou sem IdNfe.");

            var empresaKey = _configuration.GetSection("AcbrApi")["EmpresaKey"] ?? string.Empty;

            var client = _httpClientFactory.CreateClient("AcbrApi");
            var pdfBytes = await client.GetByteArrayAsync($"api/nfe/{empresaKey}/{nota.IdNfe}/cancelamento/pdf", ct);
            return File(pdfBytes, "application/pdf", $"{nota.IdNfe}.pdf");
        }

        [HttpPost("{idNotaFiscal:long}/cancelar")]
        [Authorize(Policy = "notafiscal.update")]
        public async Task<IActionResult> Cancelar(long idNotaFiscal, [FromBody] object request, CancellationToken ct)
        {
            var evento = await _service.CancelarAsync(idNotaFiscal, request, ct);
            return Ok(evento);
        }
    }
}