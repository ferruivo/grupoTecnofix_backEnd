using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class ConsumoLoteRepository : IConsumoLoteRepository
    {
        private readonly AppDbContext _db;

        public ConsumoLoteRepository(AppDbContext db) => _db = db;

        public async Task<List<ConsumoloteDto>> GetListAsync(long? idConsumolote, long? idpedidovenda, long? item, long?idlote, CancellationToken ct)
        {
            var query = _db.Consumolotes.AsNoTracking();

            if (idConsumolote != null)
            {
                query = query.Where(c => c.IdConsumolote == idConsumolote);
            }

            if (idpedidovenda != null)
            {
                query = query.Where(c => c.Idpedidovenda == idpedidovenda);
            }

            if (item != null)
            {
                query = query.Where(c => c.Item == item);
            }   

            if (idlote != null)
            {
                query = query.Where(c => c.Idlote == idlote);
            }

            return await query
                .Select(q => new ConsumoloteDto
                {
                    IdConsumolote = q.IdConsumolote,
                    Idpedidovenda = q.Idpedidovenda,
                    Item = q.Item,
                    Idproduto = q.Idproduto,
                    Idlote = q.Idlote,
                    Quantidade = q.Quantidade,
                    Data = q.Data,
                    Notafiscal = q.Notafiscal,
                    Usuario = q.Usuario
                })
                .ToListAsync(ct);

        }

        public Task<Consumolote?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Consumolotes.FirstOrDefaultAsync(x => x.IdConsumolote == id, ct);

        public Task<long?> GetSumAsync(long? idpedidovenda, long? item, CancellationToken ct)
        {
            var query = _db.Consumolotes.AsNoTracking();
            if (idpedidovenda != null)
            {
                query = query.Where(c => c.Idpedidovenda == idpedidovenda);
            }
            if (item != null)
            {
                query = query.Where(c => c.Item == item);
            }
            return query.SumAsync(c => (long?)c.Quantidade, ct);
        }
           

        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
