using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.NotaFiscal;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        private readonly AppDbContext _db;

        public NotaFiscalRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<NotaFiscalListDto>> GetListPagedAsync(
            int page,
            int pageSize,
            string? search,
            CancellationToken ct = default)
        {
            if (page <= 0)
                page = 1;

            if (pageSize <= 0)
                pageSize = 10;

            var query = _db.Set<NotaFiscal>()
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();

                query = query.Where(n =>
                    n.NumeroNota.ToString().Contains(s) ||
                    n.Serie.ToString().Contains(s) ||
                    n.Modelo.Contains(s) ||
                    (n.ChaveAcesso != null && n.ChaveAcesso.Contains(s)) ||
                    n.TipoMovimento.Contains(s) ||
                    n.TipoOperacao.Contains(s) ||
                    n.Status.Contains(s) ||
                    (n.IdNfe != null && n.IdNfe.Contains(s)));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(n => n.DataEmissao)
                .ThenByDescending(n => n.NumeroNota)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NotaFiscalListDto
                {
                    IdNotaFiscal = n.IdNotaFiscal,
                    NumeroNota = n.NumeroNota,
                    Serie = n.Serie,
                    Modelo = n.Modelo,
                    ChaveAcesso = n.ChaveAcesso,
                    DataEmissao = n.DataEmissao,
                    TipoMovimento = n.TipoMovimento,
                    TipoOperacao = n.TipoOperacao,
                    Status = n.Status,
                    ValorNota = n.ValorNota,
                    IdDestinatario = n.IdDestinatario
                })
                .ToListAsync(ct);

            return new PagedResult<NotaFiscalListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<NotaFiscal?> GetByIdAsync(
            long idNotaFiscal,
            CancellationToken ct = default)
        {
            return await _db.Set<NotaFiscal>()
                .AsNoTracking()
                .Include(x => x.NotaFiscalItems)
                    .ThenInclude(x => x.NotaFiscalItemTributos)
                .Include(x => x.NotaFiscalEventos)
                .FirstOrDefaultAsync(x => x.IdNotaFiscal == idNotaFiscal, ct);
        }

        public async Task<NotaFiscal?> GetByNumeroAsync(
            long numeroNota,
            int serie,
            string modelo,
            CancellationToken ct = default)
        {
            return await _db.Set<NotaFiscal>()
                .AsNoTracking()
                .Include(x => x.NotaFiscalItems)
                    .ThenInclude(x => x.NotaFiscalItemTributos)
                .Include(x => x.NotaFiscalEventos)
                .FirstOrDefaultAsync(x =>
                    x.NumeroNota == numeroNota &&
                    x.Serie == serie &&
                    x.Modelo == modelo, ct);
        }

        public async Task<List<NotaFiscal>> GetAllAsync(
            CancellationToken ct = default)
        {
            return await _db.Set<NotaFiscal>()
                .AsNoTracking()
                .OrderByDescending(x => x.DataEmissao)
                .ThenByDescending(x => x.NumeroNota)
                .ToListAsync(ct);
        }

        public async Task<NotaFiscal> AddAsync(
            NotaFiscal notaFiscal,
            CancellationToken ct = default)
        {
            _db.Set<NotaFiscal>().Add(notaFiscal);

            await _db.SaveChangesAsync(ct);

            return notaFiscal;
        }

        public async Task UpdateAsync(
            NotaFiscal notaFiscal,
            CancellationToken ct = default)
        {
            notaFiscal.DataAlteracao = DateTime.Now;

            _db.Set<NotaFiscal>().Update(notaFiscal);

            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(
            long idNotaFiscal,
            CancellationToken ct = default)
        {
            var notaFiscal = await _db.Set<NotaFiscal>()
                .FirstOrDefaultAsync(x => x.IdNotaFiscal == idNotaFiscal, ct);

            if (notaFiscal is null)
                return;

            _db.Set<NotaFiscal>().Remove(notaFiscal);

            await _db.SaveChangesAsync(ct);
        }

        public async Task AddItemAsync(
            NotaFiscalItem item,
            CancellationToken ct = default)
        {
            _db.Set<NotaFiscalItem>().Add(item);

            await _db.SaveChangesAsync(ct);
        }

        public async Task AddTributoItemAsync(
            NotaFiscalItemTributo tributo,
            CancellationToken ct = default)
        {
            _db.Set<NotaFiscalItemTributo>().Add(tributo);

            await _db.SaveChangesAsync(ct);
        }

        public async Task AddEventoAsync(
            NotaFiscalEvento evento,
            CancellationToken ct = default)
        {
            _db.Set<NotaFiscalEvento>().Add(evento);

            await _db.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsAsync(
            long idNotaFiscal,
            CancellationToken ct = default)
        {
            return await _db.Set<NotaFiscal>()
                .AnyAsync(x => x.IdNotaFiscal == idNotaFiscal, ct);
        }

        public async Task<NotaFiscal?> GetByIdForUpdateAsync(
    long idNotaFiscal,
    CancellationToken ct = default)
        {
            return await _db.Set<NotaFiscal>()
                .Include(x => x.NotaFiscalItems)
                    .ThenInclude(x => x.NotaFiscalItemTributos)
                .Include(x => x.NotaFiscalEventos)
                .FirstOrDefaultAsync(x => x.IdNotaFiscal == idNotaFiscal, ct);
        }

        public Task DeleteAsync(
            NotaFiscal notaFiscal,
            CancellationToken ct = default)
        {
            _db.Set<NotaFiscal>().Remove(notaFiscal);

            return Task.CompletedTask;
        }

        public async Task SaveAsync(
            CancellationToken ct = default)
        {
            await _db.SaveChangesAsync(ct);
        }
    }
}
