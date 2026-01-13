using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IUsuariosRepository
    {
        
        Task<Usuario?> GetByIdAsync(int id, CancellationToken ct);
        Task<PagedResult<UsuarioListDto>> GetListAsync(int page, int pageSize, string? search, CancellationToken ct);

        Task AddAsync(Usuario usuario, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);

        Task<List<int>> GetPerfisAsync(int idUsuario, CancellationToken ct);
        Task ReplacePerfisAsync(int idUsuario, IEnumerable<int> perfis, CancellationToken ct);
    }
}
