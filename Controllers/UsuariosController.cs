
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{

    [ApiController]
    [Route("usuarios")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _service;
        private readonly IUsuariosAclService _usuariosAcl;

        public UsuariosController(IUsuariosService service, IUsuariosAclService usuariosAcl)
        {
            _service = service;
            _usuariosAcl = usuariosAcl;
        }

        [Authorize(Policy = "usuarios.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "usuarios.read")]
        [HttpGet("lookup")]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetListAsync(search, ct));

        [Authorize(Policy = "usuarios.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioCreateDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "usuarios.update")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [Authorize(Policy = "usuarios.update")]
        [HttpPost("{id}/reset-senha")]
        public async Task<IActionResult> ResetSenha(int id, CancellationToken ct)
        {
            await _service.ResetSenhaAsync(id, ct);
            return NoContent();
        }

        [Authorize(Policy = "acl.manage")]
        [HttpGet("{id}/perfis")]
        public async Task<IActionResult> GetPerfis(int id, CancellationToken ct)
    => Ok(await _usuariosAcl.GetPerfisIdsAsync(id, ct));

        [Authorize(Policy = "acl.manage")]
        [HttpPut("{id}/perfis")]
        public async Task<IActionResult> UpdatePerfis(int id, [FromBody] List<int> perfisIds, CancellationToken ct)
        {
            await _usuariosAcl.UpdatePerfisAsync(id, perfisIds, ct);
            return NoContent();
        }
    }

}

