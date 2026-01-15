using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Dtos.Vendedor;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface ITipoDocumentoRepository
    {
        Task<List<TipoDocumentoListDto>> GetListAsync(string? search, CancellationToken ct);
    }
}
