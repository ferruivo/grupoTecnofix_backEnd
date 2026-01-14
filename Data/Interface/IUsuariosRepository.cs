using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IUsuariosRepository
    {
        Task<PagedResult<UsuarioListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<UsuarioListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<Usuario?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Usuario usuario, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
