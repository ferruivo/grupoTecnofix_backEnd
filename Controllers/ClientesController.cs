using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("clientes")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ClientesController(AppDbContext db) => _db = db;

        // GET /clientes?search=joao&page=1&pageSize=20
        //[Authorize(Policy = "clientes.read")]
        //[HttpGet]
        //public async Task<ActionResult<PagedResult<object>>> Get(
        //    [FromQuery] string? search,
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 20)
        //{
        //    if (page < 1) page = 1;
        //    if (pageSize < 1) pageSize = 20;
        //    if (pageSize > 200) pageSize = 200;

        //    var q = _db.Clientes.AsNoTracking();

        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        search = search.Trim();
        //        var digits = DocumentoUtils.OnlyDigits(search);

        //        q = q.Where(x =>
        //            x.Nome.Contains(search) ||
        //            x.Fantasia.Contains(search) ||
        //            x.Contato.Contains(search) ||
        //            x.Email.Contains(search) ||
        //            (!string.IsNullOrEmpty(digits) && (x.Cpf == digits || x.Cnpj == digits)));
        //    }

        //    var total = await q.CountAsync();

        //    var items = await q
        //        .OrderBy(x => x.Nome)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .Select(x => new
        //        {
        //            x.IdCliente,
        //            x.Nome,
        //            x.Fantasia,
        //            x.Contato,
        //            x.Email,
        //            x.Cpf,
        //            x.Cnpj,
        //            x.IdTipodocumento,
        //            x.IdMunicipio
        //        })
        //        .ToListAsync();

        //    return Ok(new PagedResult<object>(items, page, pageSize, total));
        //}

        // GET /clientes/10
        [Authorize(Policy = "clientes.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var c = await _db.Clientes.AsNoTracking()
                .Where(x => x.IdCliente == id)
                .FirstOrDefaultAsync();

            return c is null ? NotFound() : Ok(c);
        }

        // POST /clientes
        [Authorize(Policy = "clientes.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteCreateUpdate dto)
        {
            // Normalizações
            dto.Cpf = DocumentoUtils.OnlyDigits(dto.Cpf);
            dto.Cnpj = DocumentoUtils.OnlyDigits(dto.Cnpj);
            dto.Cep = DocumentoUtils.OnlyDigits(dto.Cep);
            dto.CepCobranca = DocumentoUtils.OnlyDigits(dto.CepCobranca);

            // Regras básicas
            var temCpf = !string.IsNullOrWhiteSpace(dto.Cpf);
            var temCnpj = !string.IsNullOrWhiteSpace(dto.Cnpj);

            if (temCpf && temCnpj)
                return BadRequest("Informe apenas CPF ou apenas CNPJ.");

            if (!temCpf && !temCnpj)
                return BadRequest("Informe CPF ou CNPJ.");

            if (temCpf && !DocumentoUtils.IsValidCpf(dto.Cpf!))
                return BadRequest("CPF inválido.");

            if (temCnpj && !DocumentoUtils.IsValidCnpj(dto.Cnpj!))
                return BadRequest("CNPJ inválido.");

            // Unicidade (se o banco já tem UNIQUE, isso aqui evita exception feia)
            if (temCpf)
            {
                var exists = await _db.Clientes.AnyAsync(x => x.Cpf == dto.Cpf);
                if (exists) return Conflict("Já existe cliente com este CPF.");
            }

            if (temCnpj)
            {
                var exists = await _db.Clientes.AnyAsync(x => x.Cnpj == dto.Cnpj);
                if (exists) return Conflict("Já existe cliente com este CNPJ.");
            }

            var entity = new Cliente
            {
                IdTipodocumento = dto.IdTipoDocumento,
                Cpf = temCpf ? dto.Cpf : null,
                Cnpj = temCnpj ? dto.Cnpj : null,
                InscricaoEstadual = dto.InscricaoEstadual,

                Nome = dto.Nome,
                Fantasia = dto.Fantasia,
                Contato = dto.Contato,
                Email = dto.Email,

                Cep = dto.Cep,
                Endereco = dto.Endereco,
                Bairro = dto.Bairro,
                Numero = dto.Numero,
                Complemento = dto.Complemento,
                IdMunicipio = dto.IdMunicipio,

                CepCobranca = dto.CepCobranca,
                EnderecoCobranca = dto.EnderecoCobranca,
                BairroCobranca = dto.BairroCobranca,
                NumeroCobranca = dto.NumeroCobranca,
                ComplementoCobranca = dto.ComplementoCobranca,
                IdMunicipioCobranca = dto.IdMunicipioCobranca,

                IdVendedorinterno = dto.IdVendedorInterno,
                IdVendedorexterno = dto.IdVendedorExterno,
                IdTransportadora = dto.IdTransportadora,

                DataCadastro = DateTime.UtcNow
            };

            // Se você já tem claim "sub" no JWT com IdUsuario:
            var sub = User.FindFirst("sub")?.Value;
            if (int.TryParse(sub, out var userId))
                entity.IdUsuarioCadastro = userId;

            _db.Clientes.Add(entity);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.IdCliente }, new { entity.IdCliente });
        }

        // PUT /clientes/10
        [Authorize(Policy = "clientes.update")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteCreateUpdate dto)
        {
            var c = await _db.Clientes.FindAsync(id);
            if (c is null) return NotFound();

            dto.Cpf = DocumentoUtils.OnlyDigits(dto.Cpf);
            dto.Cnpj = DocumentoUtils.OnlyDigits(dto.Cnpj);
            dto.Cep = DocumentoUtils.OnlyDigits(dto.Cep);
            dto.CepCobranca = DocumentoUtils.OnlyDigits(dto.CepCobranca);

            var temCpf = !string.IsNullOrWhiteSpace(dto.Cpf);
            var temCnpj = !string.IsNullOrWhiteSpace(dto.Cnpj);

            if (temCpf && temCnpj)
                return BadRequest("Informe apenas CPF ou apenas CNPJ.");

            if (!temCpf && !temCnpj)
                return BadRequest("Informe CPF ou CNPJ.");

            if (temCpf && !DocumentoUtils.IsValidCpf(dto.Cpf!))
                return BadRequest("CPF inválido.");

            if (temCnpj && !DocumentoUtils.IsValidCnpj(dto.Cnpj!))
                return BadRequest("CNPJ inválido.");

            // Unicidade (ignorando o próprio cliente)
            if (temCpf)
            {
                var exists = await _db.Clientes.AnyAsync(x => x.IdCliente != id && x.Cpf == dto.Cpf);
                if (exists) return Conflict("Já existe cliente com este CPF.");
            }

            if (temCnpj)
            {
                var exists = await _db.Clientes.AnyAsync(x => x.IdCliente != id && x.Cnpj == dto.Cnpj);
                if (exists) return Conflict("Já existe cliente com este CNPJ.");
            }

            c.IdTipodocumento = dto.IdTipoDocumento;
            c.Cpf = temCpf ? dto.Cpf : null;
            c.Cnpj = temCnpj ? dto.Cnpj : null;
            c.InscricaoEstadual = dto.InscricaoEstadual;

            c.Nome = dto.Nome;
            c.Fantasia = dto.Fantasia;
            c.Contato = dto.Contato;
            c.Email = dto.Email;

            c.Cep = dto.Cep;
            c.Endereco = dto.Endereco;
            c.Bairro = dto.Bairro;
            c.Numero = dto.Numero;
            c.Complemento = dto.Complemento;
            c.IdMunicipio = dto.IdMunicipio;

            c.CepCobranca = dto.CepCobranca;
            c.EnderecoCobranca = dto.EnderecoCobranca;
            c.BairroCobranca = dto.BairroCobranca;
            c.NumeroCobranca = dto.NumeroCobranca;
            c.ComplementoCobranca = dto.ComplementoCobranca;
            c.IdMunicipioCobranca = dto.IdMunicipioCobranca;

            c.IdVendedorinterno = dto.IdVendedorInterno;
            c.IdVendedorexterno = dto.IdVendedorExterno;
            c.IdTransportadora = dto.IdTransportadora;

            c.DataAlteracao = DateTime.UtcNow;
            var sub = User.FindFirst("sub")?.Value;
            if (int.TryParse(sub, out var userId))
                c.IdUsuarioAlteracao = userId;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /clientes/10
        [Authorize(Policy = "clientes.delete")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Clientes.FindAsync(id);
            if (c is null) return NotFound();

            _db.Clientes.Remove(c);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
