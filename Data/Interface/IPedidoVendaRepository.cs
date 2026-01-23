using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.PedidoVenda;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IPedidoVendaRepository
    {
        Task<PagedResult<PedidoVendaListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<PedidoVendaDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<PedidosVendum?> GetEntityByIdAsync(int id, CancellationToken ct);
        Task<int> AddAsync(PedidosVendum entity, List<PedidosVendaIten> itens, CancellationToken ct);
        Task UpdateAsync(PedidosVendum entity, List<PedidosVendaIten> itens, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
    }
}