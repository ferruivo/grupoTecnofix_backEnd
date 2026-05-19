using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class EspecificacoesRepository : IEspecificacoesRepository
    {
        private readonly AppDbContext _db;

        public EspecificacoesRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<EspecificacaoListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _db.Set<Especificacao>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(u => u.Descricao.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(e => e.Descricao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EspecificacaoListDto
                {
                    IdEspecificacao = e.IdEspecificacao,
                    Descricao = e.Descricao
                })
                .ToListAsync(ct);

            return new PagedResult<EspecificacaoListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<List<EspecificacaoListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            var query = _db.Set<Especificacao>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(u => u.Descricao.Contains(s));
            }

            var items = await query
                .OrderBy(e => e.Descricao)
                .Select(e => new EspecificacaoListDto
                {
                    IdEspecificacao = e.IdEspecificacao,
                    Descricao = e.Descricao
                })
                .ToListAsync(ct);

            return items;
        }

        public Task<Especificacao?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Set<Especificacao>().FirstOrDefaultAsync(x => x.IdEspecificacao == id, ct);

        public async Task AddAsync(Especificacao especificacao, CancellationToken ct)
            => await _db.Set<Especificacao>().AddAsync(especificacao, ct);

        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
