using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Dtos.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("fornecedores")]
    [Authorize]
    public class FornecedoresController : ControllerBase
    {
        private readonly IFornecedoresService _service;
        
        public FornecedoresController(IFornecedoresService service)
        {
            _service = service;
        }

        [Authorize(Policy = "fornecedores.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "fornecedores.read")]
        [HttpGet("lookup")]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetListAsync(search, ct));

        [Authorize(Policy = "fornecedores.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

        [Authorize(Policy = "fornecedores.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FornecedorCreateUpdateDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "fornecedores.update")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FornecedorCreateUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [Authorize(Policy = "fornecedores.read")]
        [HttpPost("export")]
        public async Task<IActionResult> Export([FromQuery] string? search = null, CancellationToken ct = default)
        {
            var bytes = await _service.ExportListToExcelAsync(search, ct);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "fornecedores.xlsx");
        }
    }
}
