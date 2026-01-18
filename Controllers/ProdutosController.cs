using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Dtos.Produto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("produtos")]
    [Authorize]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _service;

        public ProdutosController(IProdutosService service)
        {
            _service = service;
        }

        [Authorize(Policy = "produtos.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetPagedAsync(page, pageSize, search, ct));

        [Authorize(Policy = "produtos.read")]
        [HttpGet("lookup")]
        public async Task<IActionResult> Get([FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await _service.GetListAsync(search, ct));

        [Authorize(Policy = "produtos.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

        [Authorize(Policy = "produtos.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProdutoCreateUpdate dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "produtos.update")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProdutoCreateUpdate dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        #region Preço venda
        [Authorize(Policy = "precovenda.read")]
        [HttpGet("precoVenda")]
        public async Task<IActionResult> GetListPrecoVenda([FromQuery] int idCliente, CancellationToken ct = default)
        => Ok(await _service.GetListPrecoVendaAsync(idCliente, ct));

        [Authorize(Policy = "precovenda.read")]
        [HttpGet("preco-venda/{id:int}")]
        public async Task<IActionResult> GetPrecoVendaById(int idPrecoVenda, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(idPrecoVenda, ct));

        [Authorize(Policy = "precovenda.create")]
        [HttpPost("precoVenda")]
        public async Task<IActionResult> CreatePrecoVenda([FromBody] PrecoVendaCreateUpdateDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "precovenda.update")]
        [HttpPut("preco-venda/{id:int}")]
        public async Task<IActionResult> UpdatePrecoVenda(int idPrecoVenda, [FromBody] PrecoVendaCreateUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(idPrecoVenda, dto, ct);
            return NoContent();
        }

        [Authorize(Policy = "precovenda.update")]
        [HttpPut("preco-venda")]
        public async Task<IActionResult> UpdatePrecoVendaGeral([FromBody] PrecoVendaReajusteGeralDto dto, [FromQuery] decimal indiceReajuste,CancellationToken ct)
        {
            await _service.UpdateAsync(1,new ProdutoCreateUpdate { }, ct);
            return NoContent();
        }
        #endregion


    }
}
