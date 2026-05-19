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
        [HttpPost("{idEspecificacao:int}")]
        public async Task<IActionResult> Add([FromRoute] int idProduto, [FromRoute] int idEspecificacao, CancellationToken ct = default)
        {
            await _service.AddAsync(idProduto, idEspecificacao, ct);
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
