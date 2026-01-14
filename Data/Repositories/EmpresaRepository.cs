using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly AppDbContext _db;

        public EmpresaRepository(AppDbContext db) => _db = db;

        public async Task<EmpresaDto> GetAsync(string? search, CancellationToken ct)
        {
            var query = _db.Empresas.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(u =>
                    u.RazaoSocial.Contains(s) ||
                    u.NomeFantasia.Contains(s));
            }

            return await query
                .OrderBy(emp => emp.RazaoSocial)
                .Select(emp => new EmpresaDto
                {
                    IdEmpresa = emp.IdEmpresa,
                    RazaoSocial = emp.RazaoSocial,
                    NomeFantasia = emp.NomeFantasia,
                    Cnpj = emp.Cnpj,
                    InscricaoEstadual = emp.InscricaoEstadual,
                    Cep= emp.Cep,
                    Endereco = emp.Endereco,
                    Bairro =emp.Bairro,
                    Numero =emp.Numero,
                    Complemento= emp.Complemento,
                    IdMunicipio= emp.IdMunicipio,
                    Municipio = (from m in _db.Municipios
                                 where m.IdMunicipio == emp.IdMunicipio
                                 select new MunicipioDto {IdMunicipio = m.IdMunicipio, Nome = m.Nome, UF = m.Uf}).FirstOrDefault(),
                    Telefone =emp.Telefone,
                    Regime =emp.Regime,
                    AliquotaRecIcms =emp.AliquotaRecIcms
                })
                .FirstOrDefaultAsync(ct);

          


        }

        public Task<Empresa?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Empresas.FirstOrDefaultAsync(x => x.IdEmpresa == id, ct);

        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
