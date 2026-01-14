namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IUsuariosAclService
    {
        Task<List<int>> GetPerfisIdsAsync(int idUsuario, CancellationToken ct);
        Task UpdatePerfisAsync(int idUsuario, List<int> perfisIds, CancellationToken ct);
    }
}
