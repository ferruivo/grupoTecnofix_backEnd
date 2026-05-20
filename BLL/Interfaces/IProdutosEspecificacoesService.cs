using GrupoTecnofix_Api.Dtos.Especificacao;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IProdutosEspecificacoesService
    {
        Task<List<EspecificacaoListDto>> GetByProdutoAsync(int idProduto, CancellationToken ct);
        Task AddAsync(int idProduto, ProdutoEspecificacaoCreateUpdateDto dto, CancellationToken ct);
        Task DeleteAsync(int idProduto, int idEspecificacao, CancellationToken ct);
        Task UpdateAsync(int idProduto, int idEspecificacao, ProdutoEspecificacaoCreateUpdateDto dto, CancellationToken ct);
    }
}
