using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Perfil;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IPerfisService
    {
        Task<PagedResult<PerfilListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<PerfilLookupDto>> GetLookupAsync(string? search, CancellationToken ct);
        Task<int> CreateAsync(PerfilCreateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, PerfilUpdateDto dto, CancellationToken ct);
        Task<List<int>> GetPermissoesAsync(int idPerfil, CancellationToken ct);
        Task UpdatePermissoesAsync(int idPerfil, List<int> permissoesIds, CancellationToken ct);
    }
}
