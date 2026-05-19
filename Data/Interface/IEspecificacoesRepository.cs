using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IEspecificacoesRepository
    {
        Task<PagedResult<EspecificacaoListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<EspecificacaoListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<Especificacao?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Especificacao especificacao, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
