using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IPermissoesRepository
    {
        IQueryable<Permisso> Query();
        Task<Permisso?> GetAsync(int idPermissao, CancellationToken ct);
        Task<bool> ExistsAsync(int idPermissao, CancellationToken ct);
    }
}
