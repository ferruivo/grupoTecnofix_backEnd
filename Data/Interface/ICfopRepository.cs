using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface ICfopRepository
    {
        Task<List<Cfop>> GetAllAsync(CancellationToken ct);
        Task<List<Cfop>> GetListAsync(string? search, CancellationToken ct);
        Task<Cfop?> GetByIdAsync(int id, CancellationToken ct);
        Task<bool> ExistsCfopAsync(string cfop, int? ignoreId, CancellationToken ct);
        Task AddAsync(Cfop entity, CancellationToken ct);
        void Update(Cfop entity);
        void Delete(Cfop entity);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
