using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("parametrosVenda")]
    [Authorize]
    public class ParametroVendaController : ControllerBase
    {
        private readonly IParametroVendaService _service;
        
        public ParametroVendaController(IParametroVendaService service)
        {
            _service = service;
            
        }

        [Authorize(Policy = "parametrosvenda.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] long idProduto, [FromQuery] long? idCliente, CancellationToken ct = default)
            => Ok(await _service.GetAsync(idProduto, idCliente, ct));
    }
}
