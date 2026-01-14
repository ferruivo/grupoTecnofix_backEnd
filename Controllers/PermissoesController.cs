using GrupoTecnofix_Api.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("permissoes")]
    [Authorize]
    public class PermissoesController : ControllerBase
    {
        private readonly IPermissoesService _service;

        public PermissoesController(IPermissoesService service) => _service = service;

        [Authorize(Policy = "acl.manage")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
            => Ok(await _service.GetCatalogAsync(search, ct));
    }
}
