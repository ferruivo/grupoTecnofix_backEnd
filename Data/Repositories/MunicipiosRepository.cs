using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Perfis;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class MunicipiosRepository : IMunicipiosRepository
    {
        private readonly AppDbContext _db;

        public MunicipiosRepository(AppDbContext db) => _db = db;

        public async Task<List<MunicipioDto>> GetListAsync(string? search, CancellationToken ct)
        {
            var query = _db.Municipios.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(u =>
                    u.Nome.Contains(s) ||
                    u.Uf.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(m => m.Nome)
                .Select(m => new MunicipioDto
                {
                    Nome = m.Nome,
                    UF = m.Uf,
                    IdMunicipio = m.IdMunicipio,
                })
                .ToListAsync(ct);

            return items;


        }
        public Task<Municipio?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Municipios.FirstOrDefaultAsync(x => x.IdMunicipio == id, ct);
    }
}
