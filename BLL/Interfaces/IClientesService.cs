using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Dtos.Transportadoras;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IClientesService
    {
        Task<PagedResult<ClienteListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<ClienteDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(ClienteCreateUpdate dto, CancellationToken ct);
        Task UpdateAsync(int id, ClienteCreateUpdate dto, CancellationToken ct);

        Task<List<OrigemCadastroDto>> GetListOrigemAsync(string? search, CancellationToken ct);
        Task<List<TipoDocumentoDto>> GetListTipoDocumentoAsync(string? search, CancellationToken ct);

        // Export list of client DTOs to excel. Returns file bytes.
        Task<byte[]> ExportListToExcelAsync(string? search, CancellationToken ct);
    }
}
