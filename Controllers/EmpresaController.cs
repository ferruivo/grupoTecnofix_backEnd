
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("empresa")]
    [Authorize] 
    public class EmpresaController : ControllerBase
    {
        private readonly AppDbContext _db;
        public EmpresaController(AppDbContext db) => _db = db;

        [Authorize(Policy = "empresa.read")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var e = await _db.Empresas.AsNoTracking()
                .OrderBy(x => x.IdEmpresa)
                .FirstOrDefaultAsync();

            return e is null ? NotFound() : Ok(e);
        }

        [Authorize(Policy = "empresa.update")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Empresa dto)
        {
            var e = await _db.Empresas.FindAsync(id);
            if (e is null) return NotFound();

            e.RazaoSocial = dto.RazaoSocial;
            e.NomeFantasia = dto.NomeFantasia;
            e.Cnpj = dto.Cnpj;
            e.InscricaoEstadual = dto.InscricaoEstadual;
            e.Endereco = dto.Endereco;
            e.Bairro = dto.Bairro;
            e.Cep = dto.Cep;
            e.Numero = dto.Numero;
            e.Complemento = dto.Complemento;
            e.IdMunicipio = dto.IdMunicipio;
            e.Telefone = dto.Telefone;
            e.Regime = dto.Regime;
            e.AliquotaRecIcms = dto.AliquotaRecIcms;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

