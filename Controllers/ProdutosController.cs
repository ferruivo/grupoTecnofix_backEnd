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

        [Authorize(Policy = "produtos.read")]
        [HttpPost("export")]
        public async Task<IActionResult> Export([FromQuery] string? search = null, CancellationToken ct = default)
        {
            var bytes = await _service.ExportListToExcelAsync(search, ct);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "produtos.xlsx");
        }

        #region Preço venda
        [Authorize(Policy = "precovenda.read")]
        [HttpGet("precoVenda")]
        public async Task<IActionResult> GetListPrecoVenda([FromQuery] int idCliente, CancellationToken ct = default)
        => Ok(await _service.GetListPrecoVendaAsync(idCliente, ct));

        [Authorize(Policy = "precovenda.read")]
        [HttpGet("preco-venda/{id:int}")]
        public async Task<IActionResult> GetPrecoVendaById(int id, CancellationToken ct)
        => Ok(await _service.GetPrecoVendaByIdAsync(id, ct));

        [Authorize(Policy = "precovenda.create")]
        [HttpPost("precoVenda")]
        public async Task<IActionResult> CreatePrecoVenda([FromBody] PrecoVendaCreateUpdateDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "precovenda.update")]
        [HttpPut("preco-venda/{id:int}")]
        public async Task<IActionResult> UpdatePrecoVenda(int id, [FromBody] PrecoVendaCreateUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [Authorize(Policy = "precovenda.update")]
        [HttpPut("preco-venda")]
        public async Task<IActionResult> UpdatePrecoVendaGeral([FromBody] PrecoVendaReajusteGeralDto dto, [FromQuery] decimal indiceReajuste,CancellationToken ct)
        {
            await _service.UpdateAsync(1,new ProdutoCreateUpdate { }, ct);
            return NoContent();
        }
        
        [Authorize(Policy = "precovenda.read")]
        [HttpGet("preco-venda/export")]
        public async Task<IActionResult> ExportPrecoVenda([FromQuery] int idCliente, CancellationToken ct = default)
        {
            var bytes = await _service.ExportPrecoVendaToExcelAsync(idCliente, ct);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "preco-venda.xlsx");
        }
        #endregion

        #region Preço compra
        [Authorize(Policy = "precocompra.read")]
        [HttpGet("precoCompra")]
        public async Task<IActionResult> GetListPrecoCompra([FromQuery] int idFornecedor, CancellationToken ct = default)
        => Ok(await _service.GetListPrecoCompraAsync(idFornecedor, ct));

        [Authorize(Policy = "precocompra.read")]
        [HttpGet("preco-compra/{id:int}")]
        public async Task<IActionResult> GetPrecoCompraById(int id, CancellationToken ct)
        => Ok(await _service.GetPrecoCompraByIdAsync(id, ct));

        [Authorize(Policy = "precocompra.read")]
        [HttpGet("preco-compra/export")]
        public async Task<IActionResult> ExportPrecoCompra([FromQuery] int idFornecedor, CancellationToken ct = default)
        {
            var bytes = await _service.ExportPrecoCompraToExcelAsync(idFornecedor, ct);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "preco-compra.xlsx");
        }

        [Authorize(Policy = "precocompra.create")]
        [HttpPost("precoCompra")]
        public async Task<IActionResult> CreatePrecoCompra([FromBody] PrecoCompraCreateUpdateDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [Authorize(Policy = "precocompra.update")]
        [HttpPut("preco-compra/{id:int}")]
        public async Task<IActionResult> UpdatePrecoCompra(int id, [FromBody] PrecoCompraCreateUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [Authorize(Policy = "precocompra.update")]
        [HttpPut("preco-compra")]
        public async Task<IActionResult> UpdatePrecoCompraGeral([FromBody] PrecoCompraReajusteGeralDto dto, [FromQuery] decimal indiceReajuste, CancellationToken ct)
        {
            await _service.UpdateAsync(1, new ProdutoCreateUpdate { }, ct);
            return NoContent();
        }
        #endregion


    }
}
