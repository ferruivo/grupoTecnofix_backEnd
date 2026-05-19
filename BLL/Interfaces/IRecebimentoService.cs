using GrupoTecnofix_Api.Dtos.Estoque;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IRecebimentoService
    {
        Task<RecebimentoPedidoDto?> GetPedidoByNumeroAsync(
            int numero,
            CancellationToken ct);

        Task<RecebimentoLoteDto> CriarLoteAsync(
            RecebimentoLoteCreateDto dto,
            CancellationToken ct);

        Task<List<RecebimentoLoteDto>> GetLotesByPedidoAsync(
    int idPedidoCompra,
    CancellationToken ct);
    }
}
