using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Dtos.ParametroVenda;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IRecebimentoRepository
    {
        Task<RecebimentoPedidoDto?> GetPedidoByNumeroAsync(int numero,CancellationToken ct);
        Task<RecebimentoLoteDto> CriarLoteAsync(RecebimentoLoteCreateDto dto,int idUsuario,CancellationToken ct);
        Task<List<RecebimentoLoteDto>> GetLotesByPedidoAsync(int idPedidoCompra,CancellationToken ct);
        Task<List<LoteDisponivelDto>> GetLotesDisponiveisAsync(int idProduto,CancellationToken ct);
        Task<List<LoteParametroVendaDto>> GetLotesByIdProdutoAsync(long idProduto, CancellationToken ct);
        Task<EtiquetaProdutoDto> GetLoteEtiquetaByIdAsync(long idLote, CancellationToken ct);
        Task<decimal> CountByLoteAsync(long idLote, CancellationToken ct);
    }
}
