using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.PedidoVenda;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IPedidoVendaService
    {
        Task<PagedResult<PedidoVendaListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<PedidoVendaDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(PedidoVendaCreateUpdateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, PedidoVendaCreateUpdateDto dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}