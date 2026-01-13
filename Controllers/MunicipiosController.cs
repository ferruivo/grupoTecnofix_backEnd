
using GrupoTecnofix_Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("municipios")]
    [Authorize]
    public class MunicipiosController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MunicipiosController(AppDbContext db) => _db = db;

        [Authorize(Policy = "municipios.read")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? uf, [FromQuery] string? search)
        {
            var q = _db.Municipios.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(uf))
                q = q.Where(x => x.Uf == uf.Trim().ToUpper());

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                q = q.Where(x => x.Nome.Contains(search));
            }

            var items = await q
                .OrderBy(x => x.Nome)
                .Take(50)
                .Select(x => new { x.IdMunicipio, x.Nome, x.Uf })
                .ToListAsync();

            return Ok(items);
        }
    }
}
