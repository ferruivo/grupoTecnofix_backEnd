using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface ITipoDocumentoRepository
    {
        Task<List<TipoDocumentoListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<Tipodocumento?> GetByIdAsync(int id, CancellationToken ct);
    }
}
