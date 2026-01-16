using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Condição_Pagamento;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Models;


namespace GrupoTecnofix_Api.Data.Interface
{
    public interface ICondicoesPagamentoRepository
    {
        Task<PagedResult<CondicaoPagamentoListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<CondicaoPagamentoListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<Condicoespagamento?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Condicoespagamento condicaoPagamento, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
