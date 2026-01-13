
using GrupoTecnofix_Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("permissoes")]
    [Authorize(Policy = "acl.manage")]
    public class PermissoesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PermissoesController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _db.Permissoes
                .OrderBy(p => p.Codigo)
                .Select(p => new { p.IdPermissao, p.Codigo, p.Descricao })
                .ToListAsync());
    }
}
