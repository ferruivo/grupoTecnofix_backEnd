using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface INotaFiscalPreviewRepository
    {
        Task<Cfop?> GetNaturezaOperacaoAsync(int idNaturezaOperacao, CancellationToken ct);

        Task<Produto?> GetProdutoAsync(int idProduto, CancellationToken ct);
    }
}
