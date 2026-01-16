using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IFornecedoresRepository
    {
        Task<PagedResult<FornecedorListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<FornecedorListDto>> GetListAsync(string? search, CancellationToken ct);

        Task<Fornecedore?> GetByIdAsync(int id, CancellationToken ct);

        Task AddAsync(Fornecedore fornecedor, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }

}
