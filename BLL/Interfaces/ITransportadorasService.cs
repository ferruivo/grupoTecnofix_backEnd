using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface ITransportadorasService
    {
        Task<PagedResult<TransportadoraListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<TransportadoraListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<TransportadoraUpdateDto> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(TransportadoraCreateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, TransportadoraUpdateDto dto, CancellationToken ct);
    }
}
