using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Perfis;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class TransportadorasRepository : ITransportadorasRepository
    {
        private readonly AppDbContext _db;

        public TransportadorasRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<TransportadoraListDto>> GetListAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _db.Transportadoras.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(u =>
                    u.RazaoSocial.Contains(s) ||
                    u.Fantasia.Contains(s) ||
                    u.Contato.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(t => t.Fantasia)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransportadoraListDto
                {
                    IdTransportadora = t.IdTransportadora,
                    Fantasia = t.Fantasia,
                    RazaoSocial = t.RazaoSocial,
                    Contato = t.Contato,

                    Municipio = (
                        from m in _db.Municipios
                        where m.IdMunicipio == t.IdMunicipio
                        select new MunicipioDto
                        {
                            Nome = m.Nome
                        }
                    ).First()
                })
                .ToListAsync(ct);

            return new PagedResult<TransportadoraListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public Task<Transportadora?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Transportadoras.FirstOrDefaultAsync(x => x.IdTransportadora == id, ct);

        public async Task AddAsync(Transportadora transportadora, CancellationToken ct)
            => await _db.Transportadoras.AddAsync(transportadora, ct);

        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
