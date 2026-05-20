using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IProdutosEspecificacoesRepository
    {
        Task<List<EspecificacaoListDto>> GetByProdutoAsync(int idProduto, CancellationToken ct);
        Task<ProdutoEspecificacao> GetByProdutoEspecificacaoAsync(int idProduto, int idEspecificacao, CancellationToken ct);
        Task AddAsync(ProdutoEspecificacao entity, CancellationToken ct);
        Task UpdateAsync(ProdutoEspecificacao entity, CancellationToken ct);
        Task DeleteAsync(int idProduto, int idEspecificacao, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
