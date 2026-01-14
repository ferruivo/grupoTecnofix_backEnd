using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Usuario;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface ITransportadorasService
    {
        Task<PagedResult<TransportadoraListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<int> CreateAsync(TransportadoraCreateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, TransportadoraUpdateDto dto, CancellationToken ct);
    }
}
