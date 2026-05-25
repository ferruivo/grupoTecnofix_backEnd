using GrupoTecnofix_Api.Dtos.Cfop;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface ICfopService
    {
        Task<List<CfopDto>> GetAllAsync(CancellationToken ct);
        Task<List<CfopLookupDto>> GetListAsync(string? search, CancellationToken ct);
        Task<CfopDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<CfopDto> CreateAsync(CfopCreateUpdateDto dto, CancellationToken ct);
        Task<CfopDto?> UpdateAsync(int id, CfopCreateUpdateDto dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
