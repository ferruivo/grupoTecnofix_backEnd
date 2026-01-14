using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IPerfisRepository
    {
        IQueryable<Perfi> Query();
        Task<Perfi?> GetAsync(int id, CancellationToken ct);
        Task<Perfi?> GetByNomeAsync(string nome, CancellationToken ct);

        Task AddAsync(Perfi entity, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);

        // permissões do perfil
        Task<List<int>> GetPermissoesAsync(int idPerfil, CancellationToken ct);
        Task ReplacePermissoesAsync(int idPerfil, List<int> permissoesIds, CancellationToken ct);

        Task<bool> PermissoesExistemAsync(List<int> idsPermissao, CancellationToken ct);
    }
}
