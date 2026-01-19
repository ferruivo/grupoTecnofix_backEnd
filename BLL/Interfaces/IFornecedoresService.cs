using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Dtos.Usuario;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IFornecedoresService
    {
        Task<PagedResult<FornecedorListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<FornecedorListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<FornecedorDto> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(FornecedorCreateUpdateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, FornecedorCreateUpdateDto dto, CancellationToken ct);
        Task<byte[]> ExportListToExcelAsync(string? search, CancellationToken ct);
    }
}
