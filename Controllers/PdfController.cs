using Microsoft.AspNetCore.Mvc;
using GrupoTecnofix_Api.BLL.Interfaces;

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
    }
}
