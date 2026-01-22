using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IPrateleirasRepository
    {
        Task<PagedResult<Prateleira>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<Prateleira>> GetListAsync(string? search, CancellationToken ct);
        Task<Prateleira?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Prateleira prateleira, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
