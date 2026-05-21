using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Estoque;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AlocacaoMaterialsController : ControllerBase
    {
        private readonly IAlocacaoMaterialsService _service;

        public AlocacaoMaterialsController(IAlocacaoMaterialsService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Policy = "alocacaomaterial.read")]
        public async Task<ActionResult<PagedResult<AlocacaoMaterialListDto>>> GetPaged([FromQuery] int page = 1,[FromQuery] int pageSize = 20,[FromQuery] int idLote = 0,CancellationToken ct = default)
        {
            var result = await _service.GetPagedAsync(page, pageSize, idLote, ct);

            return Ok(result);
        }

        [HttpPost("salvar-alocacoes")]
        [Authorize(Policy = "alocacaomaterial.create")]
        public async Task<ActionResult<List<AlocacaoMaterialListDto>>> SalvarAlocacoes([FromBody] AlocacaoMaterialSalvarDto dto,CancellationToken ct)
        {
            var result = await _service.SalvarAlocacoesAsync(dto, ct);

            return Ok(result);
        }
    }
}
