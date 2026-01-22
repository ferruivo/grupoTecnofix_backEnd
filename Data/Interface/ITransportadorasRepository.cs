using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Transportadora;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface ITransportadorasRepository
    {
        Task<PagedResult<TransportadoraListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<TransportadoraListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<List<TransportadoraExcelDto>> GetListExcelAsync(string? search, CancellationToken ct);
        Task<Transportadora?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Transportadora transportadora, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
