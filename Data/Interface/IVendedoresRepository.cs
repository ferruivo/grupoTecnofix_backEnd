using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IVendedoresRepository
    {
        Task<PagedResult<VendedorListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<VendedorListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<VendedorDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<Vendedore?> GetAsync(int id, CancellationToken ct);
        Task AddAsync(Vendedore vendedore, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
