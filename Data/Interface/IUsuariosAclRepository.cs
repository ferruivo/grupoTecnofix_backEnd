namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IUsuariosAclRepository
    {
        Task<bool> UsuarioExistsAsync(int idUsuario, CancellationToken ct);
        Task<bool> PerfisExistemAsync(List<int> idsPerfis, CancellationToken ct);

        Task<List<int>> GetPerfisIdsAsync(int idUsuario, CancellationToken ct);
        Task ReplacePerfisAsync(int idUsuario, List<int> perfisIds, CancellationToken ct);

        Task SaveAsync(CancellationToken ct);
    }
}
