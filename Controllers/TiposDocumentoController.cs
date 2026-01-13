
using GrupoTecnofix_Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("tipos-documento")]
    [Authorize]
    public class TiposDocumentoController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TiposDocumentoController(AppDbContext db) => _db = db;

        [Authorize(Policy = "tipodocumento.read")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _db.Tipodocumentos.AsNoTracking()
                .OrderBy(x => x.Descricao)
                .Select(x => new { x.IdTipodocumento, x.Descricao })
                .ToListAsync();

            return Ok(items);
        }
    }
}
