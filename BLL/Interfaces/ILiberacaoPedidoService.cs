using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Dtos.PedidoVenda;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface ILiberacaoPedidoService
    {
        Task<PagedResult<PedidoVendaPendenteItemDto>> GetPendentesLiberacaoAsync(
            int idCliente,
            int page,
            int pageSize,
            CancellationToken ct);

        Task<List<LoteDisponivelDto>> GetLotesDisponiveisAsync(
            int idProduto,
            CancellationToken ct);

        Task<LiberarPendenteResponseDto> LiberarPendenteAsync(
            LiberarPendenteRequestDto dto,
            CancellationToken ct);

        Task<List<LiberacaoAlocacoesDto>> GetAlocacoesLiberadasAsync(
    int idPedidoVenda,
    int? item,
    CancellationToken ct);

        Task<EstornarLiberacaoResponseDto> EstornarLiberacaoAsync(
            EstornarLiberacaoRequestDto dto,
            CancellationToken ct);

        Task<ExpedicaoConsumoLoteDto>GetExpedicaoConsumoLoteAsync(
    int idCliente,
    CancellationToken ct);
        
    }
}
