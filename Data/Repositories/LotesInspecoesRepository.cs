using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class LotesInspecoesRepository : ILotesInspecoesRepository
    {
        private readonly AppDbContext _db;

        public LotesInspecoesRepository(AppDbContext db) => _db = db;

        public async Task<List<LoteInspecao>> GetByLoteAsync(int idLote, CancellationToken ct)
        {
            return await _db.Set<LoteInspecao>().AsNoTracking().Where(x => x.IdLote == idLote).ToListAsync(ct);
        }

        public Task<LoteInspecao?> GetByLoteEspecificacaoAsync(int idLote, int idEspecificacao, CancellationToken ct)
            => _db.Set<LoteInspecao>().FirstOrDefaultAsync(x => x.IdLote == idLote && x.IdEspecificacao == idEspecificacao, ct);

        public async Task AddAsync(LoteInspecao entity, CancellationToken ct)
            => await _db.Set<LoteInspecao>().AddAsync(entity, ct);

        public async Task DeleteAsync(int idLoteInspecao, CancellationToken ct)
        {
            var it = await _db.Set<LoteInspecao>().FirstOrDefaultAsync(x => x.IdLoteInspecao == idLoteInspecao, ct);
            if (it != null) _db.Remove(it);
        }

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
