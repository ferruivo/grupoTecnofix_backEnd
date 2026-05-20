using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.Especificacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("produtos/{idProduto:int}/especificacoes")]
    [Authorize]
    public class ProdutosEspecificacoesController : ControllerBase
    {
        private readonly IProdutosEspecificacoesService _service;

        public ProdutosEspecificacoesController(IProdutosEspecificacoesService service) => _service = service;

        [Authorize(Policy = "especificacoes.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] int idProduto, CancellationToken ct = default)
            => Ok(await _service.GetByProdutoAsync(idProduto, ct));

        [Authorize(Policy = "especificacoes.update")]
        [HttpPost]
        public async Task<IActionResult> Add([FromRoute] int idProduto, [FromBody] ProdutoEspecificacaoCreateUpdateDto dto, CancellationToken ct = default)
        {
            if (dto == null) return BadRequest();

            await _service.AddAsync(idProduto, dto, ct);
            return NoContent();
        }

        [Authorize(Policy = "especificacoes.update")]
        [HttpPut("{idEspecificacao:int}")]
        public async Task<IActionResult> Update([FromRoute] int idProduto, [FromRoute] int idEspecificacao, [FromBody] ProdutoEspecificacaoCreateUpdateDto dto, CancellationToken ct = default)
        {
            if (dto == null) return BadRequest();

            await _service.UpdateAsync(idProduto, idEspecificacao, dto, ct);
            return NoContent();
        }

        [Authorize(Policy = "especificacoes.update")]
        [HttpDelete("{idEspecificacao:int}")]
        public async Task<IActionResult> Delete([FromRoute] int idProduto, [FromRoute] int idEspecificacao, CancellationToken ct = default)
        {
            await _service.DeleteAsync(idProduto, idEspecificacao, ct);
            return NoContent();
        }
    }
}
