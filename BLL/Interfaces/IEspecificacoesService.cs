using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Especificacao;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IEspecificacoesService
    {
        Task<PagedResult<EspecificacaoListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<EspecificacaoListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<EspecificacaoDto> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(EspecificacaoDto dto, CancellationToken ct);
        Task UpdateAsync(int id, EspecificacaoDto dto, CancellationToken ct);
    }
}
