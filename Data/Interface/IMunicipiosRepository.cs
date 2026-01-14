using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IMunicipiosRepository
    {
        Task<List<MunicipioDto>> GetListAsync(string? search, CancellationToken ct);
        Task<Municipio?> GetByIdAsync(int id, CancellationToken ct);
    }
}
