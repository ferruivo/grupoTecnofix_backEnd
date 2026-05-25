using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class CfopRepository : ICfopRepository
    {
        private readonly AppDbContext _context;

        public CfopRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cfop>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Cfops
                .AsNoTracking()
                .OrderBy(x => x.Cfop1)
                .ToListAsync(ct);
        }

        public async Task<List<Cfop>> GetListAsync(string? search, CancellationToken ct)
        {
            var query = _context.Cfops
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Cfop1.Contains(search) ||
                    x.Descricao.Contains(search));
            }

            return await query
                .OrderBy(x => x.Cfop1)
                .Take(50)
                .ToListAsync(ct);
        }

        public async Task<Cfop?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Cfops
                .FirstOrDefaultAsync(x => x.IdCfop == id, ct);
        }

        public async Task<bool> ExistsCfopAsync(string cfop, int? ignoreId, CancellationToken ct)
        {
            return await _context.Cfops.AnyAsync(x =>
                x.Cfop1 == cfop &&
                (!ignoreId.HasValue || x.IdCfop != ignoreId.Value), ct);
        }

        public async Task AddAsync(Cfop entity, CancellationToken ct)
        {
            await _context.Cfops.AddAsync(entity, ct);
        }

        public void Update(Cfop entity)
        {
            _context.Cfops.Update(entity);
        }

        public void Delete(Cfop entity)
        {
            _context.Cfops.Remove(entity);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
