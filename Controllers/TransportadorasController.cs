
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("transportadoras")]
    [Authorize]
    public class TransportadorasController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TransportadorasController(AppDbContext db) => _db = db;

        //[Authorize(Policy = "transportadoras.read")]
        //[HttpGet]
        //public async Task<ActionResult<PagedResult<object>>> Get(
        //    [FromQuery] string? search,
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 20)
        //{
        //    page = Paging.ClampPage(page);
        //    pageSize = Paging.ClampPageSize(pageSize);

        //    var q = _db.Transportadoras.AsNoTracking();

        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        search = search.Trim();
        //        q = q.Where(x =>
        //            x.RazaoSocial.Contains(search) ||
        //            x.Fantasia.Contains(search) ||
        //            x.Cnpj.Contains(search));
        //    }

        //    var total = await q.CountAsync();

        //    var items = await q
        //        .OrderBy(x => x.RazaoSocial)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .Select(x => new
        //        {
        //            x.IdTransportadora,
        //            x.Cnpj,
        //            x.RazaoSocial,
        //            x.Fantasia,
        //            x.Contato,
        //            x.Telefone,
        //            x.Cep,
        //            x.Endereco,
        //            x.Bairro,
        //            x.Numero,
        //            x.Complemento,
        //            x.IdMunicipio
        //        })
        //        .ToListAsync();

        //    return Ok(new PagedResult<object>(items, page, pageSize, total));
        //}

        [Authorize(Policy = "transportadoras.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.Transportadoras.AsNoTracking()
                .Where(x => x.IdTransportadora == id)
                .Select(x => new
                {
                    x.IdTransportadora,
                    x.Cnpj,
                    x.InscricaoEstadual,
                    x.RazaoSocial,
                    x.Fantasia,
                    x.Contato,
                    x.Telefone,
                    x.Cep,
                    x.Endereco,
                    x.Bairro,
                    x.Numero,
                    x.Complemento,
                    x.IdMunicipio
                })
                .FirstOrDefaultAsync();

            return item is null ? NotFound() : Ok(item);
        }

        [Authorize(Policy = "transportadoras.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Transportadora dto)
        {
            _db.Transportadoras.Add(dto);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dto.IdTransportadora }, dto);
        }

        [Authorize(Policy = "transportadoras.update")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Transportadora dto)
        {
            var e = await _db.Transportadoras.FindAsync(id);
            if (e is null) return NotFound();

            e.Cnpj = dto.Cnpj;
            e.InscricaoEstadual = dto.InscricaoEstadual;
            e.RazaoSocial = dto.RazaoSocial;
            e.Fantasia = dto.Fantasia;
            e.Contato = dto.Contato;
            e.Telefone = dto.Telefone;
            e.Cep = dto.Cep;
            e.Endereco = dto.Endereco;
            e.Bairro = dto.Bairro;
            e.Numero = dto.Numero;
            e.Complemento = dto.Complemento;
            e.IdMunicipio = dto.IdMunicipio;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Policy = "transportadoras.delete")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var e = await _db.Transportadoras.FindAsync(id);
            if (e is null) return NotFound();

            _db.Transportadoras.Remove(e);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
