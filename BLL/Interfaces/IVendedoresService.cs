using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IVendedoresService
    {
        Task<PagedResult<VendedorListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<VendedorListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<VendedorDto> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(VendedorCreateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, VendedorUpdateDto dto, CancellationToken ct);
    }
}
