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
            var items = await (
                from pe in _db.Set<ProdutoEspecificacao>().AsNoTracking()
                join e in _db.Especificacoes.AsNoTracking()
                    on pe.IdEspecificacao equals e.IdEspecificacao
                where pe.IdProduto == idProduto
                orderby e.Descricao
                select new EspecificacaoListDto
                {
                    IdEspecificacao = e.IdEspecificacao,
                    Descricao = e.Descricao,

                    Minimo = pe.Minimo,
                    Maximo = pe.Maximo,
                    Aproximado = pe.Aproximado,
                    Observacao = pe.Observacao
                }
            ).ToListAsync(ct);

            return items;
        }

        public async Task<ProdutoEspecificacao> GetByProdutoEspecificacaoAsync(int idProduto, int idEspecificacao, CancellationToken ct)
        {
            var item = await (from pe in _db.Set<ProdutoEspecificacao>().AsNoTracking()
                              where pe.IdProduto == idProduto && pe.IdEspecificacao == idEspecificacao
                              select pe).FirstOrDefaultAsync(ct);

            return item;
        }
        public async Task AddAsync(ProdutoEspecificacao entity, CancellationToken ct)
        {
            await _db.Set<ProdutoEspecificacao>().AddAsync(entity, ct); // No changes made
        }

        public Task UpdateAsync(ProdutoEspecificacao entity, CancellationToken ct)
        {
            _db.Set<ProdutoEspecificacao>().Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int idProduto, int idEspecificacao, CancellationToken ct)
        {
            var pe = await _db.Set<ProdutoEspecificacao>().FirstOrDefaultAsync(x => x.IdProduto == idProduto && x.IdEspecificacao == idEspecificacao, ct);
            if (pe != null) _db.Remove(pe);
        }

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
