namespace GrupoTecnofix_Api.Auth
{
    public interface IPermissionService
    {
        Task<List<string>> GetPerfisAsync(int idUsuario);
        Task<List<string>> GetPermissoesAsync(int idUsuario);
    }
}
