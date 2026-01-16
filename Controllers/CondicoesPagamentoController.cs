using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("condicoespagamento")]
    [Authorize]
    public class CondicoesPagamentoController : ControllerBase
    {
        private readonly ICondicoesPagamentoService _service;
        

        public CondicoesPagamentoController(ICondicoesPagamentoService service)
        {
            _service = service;
        }

        [Authorize(Policy = "condicoespagamento.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "condicoespagamento.read")]
        [HttpGet("lookup")]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetListAsync(search, ct));

        [Authorize(Policy = "condicoespagamento.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

        [Authorize(Policy = "condicoespagamento.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CondicaoPagamentoDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "condicoespagamento.update")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CondicaoPagamentoDto dto, CancellationToken ct)
        {
            dto.IdCondicoespagamento = id;
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }
    }
}
