using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Condição_Pagamento;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Perfis;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class CondicoesPagamentoRepository : ICondicoesPagamentoRepository
    {
        private readonly AppDbContext _db;

        public CondicoesPagamentoRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Condicoespagamento condicoespagamento, CancellationToken ct)
            => await _db.Condicoespagamentos.AddAsync(condicoespagamento, ct);

        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public Task<Condicoespagamento?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Condicoespagamentos.FirstOrDefaultAsync(x => x.IdCondicoespagamento == id, ct);

        public async Task<List<CondicaoPagamentoListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            var query = _db.Condicoespagamentos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(u =>
                    u.Descricao.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(c => c.Descricao)
                .Select(c => new CondicaoPagamentoListDto
                {
                    IdCondicaoPagamento = c.IdCondicoespagamento,
                    Descricao = c.Descricao
                })
                .ToListAsync(ct);

            return items;
        }

        public async Task<PagedResult<CondicaoPagamentoListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _db.Condicoespagamentos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(u =>
                    u.Descricao.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(c => c.Descricao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CondicaoPagamentoListDto
                {
                    IdCondicaoPagamento = c.IdCondicoespagamento,
                    Descricao = c.Descricao
                })
                .ToListAsync(ct);

            return new PagedResult<CondicaoPagamentoListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }
    }
}
