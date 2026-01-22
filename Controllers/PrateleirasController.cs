using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("prateleiras")]
    [Authorize]
    public class PrateleirasController : ControllerBase
    {
        private readonly IPrateleirasService _service;
       
        public PrateleirasController(IPrateleirasService service)
        {
            _service = service;
        }

        [Authorize(Policy = "prateleiras.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "prateleiras.read")]
        [HttpGet("lookup")]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetListAsync(search, ct));

        [Authorize(Policy = "prateleiras.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

        [Authorize(Policy = "prateleiras.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Prateleira dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "prateleiras.update")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Prateleira dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return Ok();
        }

    }
}
