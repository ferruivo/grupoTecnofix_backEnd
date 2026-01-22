using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.PedidoCompra;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IPedidoCompraService
    {
        Task<PagedResult<PedidoCompraListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<PedidoCompraDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(PedidoCompraCreateUpdateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, PedidoCompraCreateUpdateDto dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
