using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class ProdutosEspecificacoesRepository : IProdutosEspecificacoesRepository
    {
        private readonly AppDbContext _db;

        public ProdutosEspecificacoesRepository(AppDbContext db) => _db = db;

        public async Task<List<EspecificacaoListDto>> GetByProdutoAsync(int idProduto, CancellationToken ct)
        {
            var items = await _db.Especificacoes
                .AsNoTracking()
                .Where(e => _db.Set<ProdutoEspecificacao>().Any(pe => pe.IdProduto == idProduto && pe.IdEspecificacao == e.IdEspecificacao))
                .OrderBy(e => e.Descricao)
                .Select(e => new EspecificacaoListDto { IdEspecificacao = e.IdEspecificacao, Descricao = e.Descricao })
                .ToListAsync(ct);

            return items;
        }

        public async Task AddAsync(ProdutoEspecificacao entity, CancellationToken ct)
        {
            await _db.Set<ProdutoEspecificacao>().AddAsync(entity, ct);
        }

        public async Task DeleteAsync(int idProduto, int idEspecificacao, CancellationToken ct)
        {
            var pe = await _db.Set<ProdutoEspecificacao>().FirstOrDefaultAsync(x => x.IdProduto == idProduto && x.IdEspecificacao == idEspecificacao, ct);
            if (pe != null) _db.Remove(pe);
        }

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
