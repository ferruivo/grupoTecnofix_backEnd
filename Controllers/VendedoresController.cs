
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("vendedores")]
    [Authorize]
    public class VendedoresController : ControllerBase
    {
        private readonly AppDbContext _db;
        public VendedoresController(AppDbContext db) => _db = db;

        //[Authorize(Policy = "vendedores.read")]
        //[HttpGet]
        //public async Task<ActionResult<PagedResult<object>>> Get(
        //    [FromQuery] string? tipo, // interno|externo (lookup)
        //    [FromQuery] string? search,
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 20)
        //{
        //    page = Paging.ClampPage(page);
        //    pageSize = Paging.ClampPageSize(pageSize);

        //    var q = _db.Vendedores.AsNoTracking();

        //    if (tipo?.ToLower() == "interno") q = q.Where(x => x.Interno);
        //    if (tipo?.ToLower() == "externo") q = q.Where(x => x.Externo);

        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        search = search.Trim();
        //        // se quiser pesquisar por usuário, precisa Join com Usuarios (abaixo)
        //    }

        //    var total = await q.CountAsync();

        //    var items = await q
        //        .OrderBy(x => x.IdVendedor)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .Select(x => new
        //        {
        //            x.IdVendedor,
        //            x.IdUsuario,
        //            x.Interno,
        //            x.Externo,
        //            x.Observacao
        //        })
        //        .ToListAsync();

        //    return Ok(new PagedResult<object>(items, page, pageSize, total));
        //}

        [Authorize(Policy = "vendedores.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.Vendedores.AsNoTracking()
                .Where(x => x.IdVendedor == id)
                .Select(x => new
                {
                    x.IdVendedor,
                    x.IdUsuario,
                    x.Interno,
                    x.Externo,
                    x.Observacao
                })
                .FirstOrDefaultAsync();

            return item is null ? NotFound() : Ok(item);
        }

        [Authorize(Policy = "vendedores.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vendedore dto)
        {
            _db.Vendedores.Add(dto);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dto.IdVendedor }, dto);
        }

        [Authorize(Policy = "vendedores.update")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Vendedore dto)
        {
            var e = await _db.Vendedores.FindAsync(id);
            if (e is null) return NotFound();

            e.IdUsuario = dto.IdUsuario;
            e.Interno = dto.Interno;
            e.Externo = dto.Externo;
            e.Observacao = dto.Observacao;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Policy = "vendedores.delete")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var e = await _db.Vendedores.FindAsync(id);
            if (e is null) return NotFound();

            _db.Vendedores.Remove(e);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
