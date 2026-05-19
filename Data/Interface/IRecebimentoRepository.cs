using GrupoTecnofix_Api.Dtos.Estoque;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IRecebimentoRepository
    {
        Task<RecebimentoPedidoDto?> GetPedidoByNumeroAsync(
            int numero,
            CancellationToken ct);

        Task<RecebimentoLoteDto> CriarLoteAsync(
            RecebimentoLoteCreateDto dto,
            int idUsuario,
            CancellationToken ct);

        Task<List<RecebimentoLoteDto>> GetLotesByPedidoAsync(
    int idPedidoCompra,
    CancellationToken ct);
    }
}
