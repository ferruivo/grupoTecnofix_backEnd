using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Condição_Pagamento;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Usuario;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface ICondicoesPagamentoService
    {
        Task<PagedResult<CondicaoPagamentoListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<CondicaoPagamentoListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<CondicaoPagamentoDto> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(CondicaoPagamentoDto dto, CancellationToken ct);
        Task UpdateAsync(int id, CondicaoPagamentoDto dto, CancellationToken ct);
    }
}
