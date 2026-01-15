using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Dtos.Vendedor;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface ITipoDocumentoService
    {
        Task<List<TipoDocumentoListDto>> GetListAsync(string? search, CancellationToken ct);
    }
}
