using GrupoTecnofix_Api.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("tiposDocumento")]
    [Authorize]
    public class TiposDocumentoController : ControllerBase
    {
        private readonly ITipoDocumentoService _service;

        public TiposDocumentoController(ITipoDocumentoService service) => _service = service;

        [Authorize(Policy = "tipodocumento.read")]
        [HttpGet("lookup")]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetListAsync(search, ct));
    }
}
