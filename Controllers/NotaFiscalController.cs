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

        public NotaFiscalController(INotaFiscalService service, INotaFiscalPreviewService previewService)
        {
            _service = service;
            _previewService = previewService;
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
        public async Task<IActionResult> GerarPreview(
    [FromBody] NotaFiscalPreviewRequestDto dto,
    CancellationToken ct)
        {
            var result = await _previewService.GerarPreviewAsync(dto, ct);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "notafiscal.create")]
        public async Task<IActionResult> Create(
            [FromBody] NotaFiscalCreateDto dto,
            CancellationToken ct)
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
        public async Task<IActionResult> Update(
            long idNotaFiscal,
            [FromBody] NotaFiscalUpdateDto dto,
            CancellationToken ct)
        {
            await _service.UpdateAsync(idNotaFiscal, dto, ct);

            return Ok(new
            {
                message = "Nota fiscal atualizada com sucesso."
            });
        }

        [HttpDelete("{idNotaFiscal:long}")]
        [Authorize(Policy = "notafiscal.delete")]
        public async Task<IActionResult> Delete(
            long idNotaFiscal,
            CancellationToken ct)
        {
            await _service.DeleteAsync(idNotaFiscal, ct);

            return Ok(new
            {
                message = "Nota fiscal excluída com sucesso."
            });
        }

        [HttpPost("{idNotaFiscal:long}/eventos")]
        [Authorize(Policy = "notafiscal.create")]
        public async Task<IActionResult> AddEvento(
            long idNotaFiscal,
            [FromBody] NotaFiscalEventoCreateDto dto,
            CancellationToken ct)
        {
            dto.IdNotaFiscal = idNotaFiscal;

            await _service.AddEventoAsync(dto, ct);

            return Ok(new
            {
                message = "Evento registrado com sucesso."
            });
        }

        [HttpPost("{idNotaFiscal:long}/emitir")]
        [Authorize(Policy = "notafiscal.create")]
        public async Task<IActionResult> Emitir(
            long idNotaFiscal,
            CancellationToken ct)
        {
            await _service.EmitirAsync(idNotaFiscal, ct);

            return Ok(new
            {
                message = "Nota fiscal enviada para emissão."
            });
        }

        [HttpPost("importar-itens-preview")]
        [Authorize(Policy = "notafiscal.create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportarItensPreview(
    [FromForm] NotaFiscalImportarItensFormDto form,
    CancellationToken ct)
        {
            var result = await _service.ImportarItensPreviewAsync(form.Arquivo, ct);
            return Ok(result);
        }
    }
}