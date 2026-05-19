using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.Especificacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("especificacoes")]
    [Authorize]
    public class EspecificacoesController : ControllerBase
    {
        private readonly IEspecificacoesService _service;

        public EspecificacoesController(IEspecificacoesService service) => _service = service;

        [Authorize(Policy = "especificacoes.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
            => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "especificacoes.read")]
        [HttpGet("lookup")]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
            => Ok(await _service.GetListAsync(search, ct));

        [Authorize(Policy = "especificacoes.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => Ok(await _service.GetByIdAsync(id, ct));

        [Authorize(Policy = "especificacoes.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EspecificacaoDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "especificacoes.update")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EspecificacaoDto dto, CancellationToken ct)
        {
            dto.IdEspecificacao = id;
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }
    }
}
