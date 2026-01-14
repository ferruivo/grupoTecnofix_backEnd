using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Usuario;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IMunicipiosService
    {
        Task<List<MunicipioDto>> GetListAsync(string? search, CancellationToken ct);
    }
}
