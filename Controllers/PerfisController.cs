
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos.Perfil;
using GrupoTecnofix_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("perfis")]
    [Authorize]
    public class PerfisController : ControllerBase
    {
        private readonly IPerfisService _service;

        public PerfisController(IPerfisService service) => _service = service;

        [Authorize(Policy = "acl.manage")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
            => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "acl.manage")]
        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? search = null, CancellationToken ct = default)
            => Ok(await _service.GetLookupAsync(search, ct));

        [Authorize(Policy = "acl.manage")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PerfilCreateDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return Ok(new { id });
        }

        [Authorize(Policy = "acl.manage")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PerfilUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        // =========================
        // PERMISSÕES DO PERFIL
        // =========================
        [Authorize(Policy = "acl.manage")]
        [HttpGet("{id}/permissoes")]
        public async Task<IActionResult> GetPermissoes(int id, CancellationToken ct)
    => Ok(await _service.GetPermissoesAsync(id, ct));

        [Authorize(Policy = "acl.manage")]
        [HttpPut("{id}/permissoes")]
        public async Task<IActionResult> UpdatePermissoes(int id, [FromBody] List<int> permissoesIds, CancellationToken ct)
        {
            await _service.UpdatePermissoesAsync(id, permissoesIds, ct);
            return NoContent();
        }
    }
}
