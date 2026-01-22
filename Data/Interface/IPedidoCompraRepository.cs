using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.PedidoCompra;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IPedidoCompraRepository
    {
        Task<PagedResult<PedidoCompraListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<PedidoCompraDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<PedidosCompra?> GetEntityByIdAsync(int id, CancellationToken ct);
        Task<int> AddAsync(PedidosCompra entity, List<PedidosCompraIten> itens, CancellationToken ct);
        Task UpdateAsync(PedidosCompra entity, List<PedidosCompraIten> itens, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
