using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Usuario;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IUsuariosService
    {
        Task<PagedResult<UsuarioListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);

        Task<int> CreateAsync(UsuarioCreateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, UsuarioUpdateDto dto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);

        Task<List<int>> GetPerfisAsync(int id, CancellationToken ct);
        Task UpdatePerfisAsync(int id, List<int> perfis, CancellationToken ct);

        Task ResetSenhaAsync(int id, CancellationToken ct);
    }
}
