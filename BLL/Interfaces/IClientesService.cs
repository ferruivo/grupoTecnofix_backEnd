using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Transportadoras;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IClientesService
    {
        Task<PagedResult<ClienteListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<ClienteDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(ClienteCreateUpdate dto, CancellationToken ct);
        Task UpdateAsync(int id, ClienteCreateUpdate dto, CancellationToken ct);
    }
}
