using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class NotaFiscalPreviewRepository : INotaFiscalPreviewRepository
    {
        private readonly AppDbContext _db;

        public NotaFiscalPreviewRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Cfop?> GetNaturezaOperacaoAsync(
            int idNaturezaOperacao,
            CancellationToken ct)
        {
            return await _db.Set<Cfop>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdCfop == idNaturezaOperacao, ct);
        }

        public async Task<Produto?> GetProdutoAsync(
            int idProduto,
            CancellationToken ct)
        {
            return await _db.Set<Produto>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdProduto == idProduto, ct);
        }
    }
}
