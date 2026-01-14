using GrupoTecnofix_Api.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("municipios")]
    [Authorize]
    public class MunicipiosController : ControllerBase
    {
        private readonly IMunicipiosService _service;

        public MunicipiosController(IMunicipiosService service) => _service = service;

        [Authorize(Policy = "municipios.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetListAsync(search, ct));
    }
}
