using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IAlocacaoMaterialsRepository
    {
        Task<PagedResult<AlocacaoMaterialListDto>> GetListPagedAsync(int page, int pageSize, int idLote, CancellationToken ct);
        Task<AlocacaoMaterial?> GetByIdAsync(int id, CancellationToken ct);
        Task<List<AlocacaoMaterial>> GetByLoteAsync(int idLote, CancellationToken ct);
        Task AddAsync(AlocacaoMaterial entity, CancellationToken ct);
        void Remove(AlocacaoMaterial entity);
        Task SaveAsync(CancellationToken ct);
    }
}
