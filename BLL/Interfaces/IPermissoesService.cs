using GrupoTecnofix_Api.Dtos.Perfil;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IPermissoesService
    {
        Task<List<PermissaoDto>> GetCatalogAsync(string? search, CancellationToken ct);
    }
}
