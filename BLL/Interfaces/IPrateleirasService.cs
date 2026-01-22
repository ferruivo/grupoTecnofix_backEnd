using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IPrateleirasService
    {
        Task<PagedResult<Prateleira>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<Prateleira>> GetListAsync(string? search, CancellationToken ct);
        Task<Prateleira> GetByIdAsync(int id, CancellationToken ct);
        Task<int?> CreateAsync(Prateleira dto, CancellationToken ct);
        Task UpdateAsync(int id, Prateleira dto, CancellationToken ct);
    }
}
