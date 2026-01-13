
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("perfis")]
    [Authorize(Policy = "acl.manage")]
    public class PerfisController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PerfisController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _db.Perfis
                .Select(p => new { p.IdPerfil, p.Nome })
                .OrderBy(p => p.Nome)
                .ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Perfi dto)
        {
            _db.Perfis.Add(dto);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { dto.IdPerfil }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Perfi dto)
        {
            var perfil = await _db.Perfis.FindAsync(id);
            if (perfil == null) return NotFound();

            perfil.Nome = dto.Nome;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var perfil = await _db.Perfis.FindAsync(id);
            if (perfil == null) return NotFound();

            _db.Perfis.Remove(perfil);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // =========================
        // PERMISSÕES DO PERFIL
        // =========================

        [HttpGet("{id}/permissoes")]
        public async Task<IActionResult> GetPermissoes(int id)
        {
            var permissoes = await (from pp in _db.PerfisPermissoes
                                    join p in _db.Permissoes on pp.IdPermissao equals p.IdPermissao
                                    where pp.IdPerfil == id
                                    select new { p.IdPermissao, p.Codigo }).ToListAsync();

            return Ok(permissoes);
        }

        [HttpPut("{id}/permissoes")]
        public async Task<IActionResult> UpdatePermissoes(int id, [FromBody] List<int> permissoes)
        {
            var atuais = _db.PerfisPermissoes.Where(x => x.IdPerfil == id);
            _db.PerfisPermissoes.RemoveRange(atuais);

            foreach (var pid in permissoes.Distinct())
            {
                _db.PerfisPermissoes.Add(new PerfisPermisso
                {
                    IdPerfil = id,
                    IdPermissao = pid
                });
            }

            await _db.SaveChangesAsync();
            return NoContent();
            }
        }
}
