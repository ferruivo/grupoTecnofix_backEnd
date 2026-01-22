using DocumentFormat.OpenXml.Wordprocessing;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Perfis;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class PrateleirasRepository : IPrateleirasRepository
    {
        private readonly AppDbContext _db;

        public PrateleirasRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<Prateleira>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _db.Prateleiras.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(p =>
                    p.Descricao.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(p => p.Descricao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new Prateleira
                {
                    IdPrateleira = p.IdPrateleira,
                    Descricao = p.Descricao,

                })
                .ToListAsync(ct);

            return new PagedResult<Prateleira>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<List<Prateleira>> GetListAsync(string? search, CancellationToken ct)
        {
            var query = _db.Prateleiras.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(p =>
                    p.Descricao.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .Select(p => new Prateleira
                {
                    IdPrateleira = p.IdPrateleira,
                    Descricao = p.Descricao,

                })
                .ToListAsync(ct);

            return items;


        }

        public Task<Prateleira?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Prateleiras.FirstOrDefaultAsync(x => x.IdPrateleira == id, ct);

        public async Task AddAsync(Prateleira prateleira, CancellationToken ct)
            => await _db.Prateleiras.AddAsync(prateleira, ct);

        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
