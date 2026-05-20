using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Dtos.ParametroVenda;
using GrupoTecnofix_Api.Dtos.PedidoVenda;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IPedidoVendaRepository
    {
        Task<PagedResult<PedidoVendaListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<PedidoVendaDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<PedidosVendum?> GetEntityByIdAsync(int id, CancellationToken ct);
        Task<int> AddAsync(PedidosVendum entity, List<PedidosVendaIten> itens, CancellationToken ct);
        Task UpdateAsync(PedidosVendum entity, List<PedidosVendaIten> itens, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
        Task<PagedResult<PedidoVendaPendenteItemDto>> GetPendentesLiberacaoAsync(
            int idCliente,
            int page,
            int pageSize,
            CancellationToken ct);
        Task<LiberarPendenteResponseDto> LiberarPendenteAsync(LiberarPendenteRequestDto dto, int? idUsuario, CancellationToken ct);
        Task<List<LiberacaoAlocacoesDto>> GetAlocacoesLiberadasAsync(int idPedidoVenda,int? item,CancellationToken ct);
        Task<EstornarLiberacaoResponseDto> EstornarLiberacaoAsync(EstornarLiberacaoRequestDto dto,CancellationToken ct);
        Task <ExpedicaoConsumoLoteDto> GetExpedicaoConsumoLoteAsync(int idCliente, CancellationToken ct);
        Task<List<PedidoVendaDisponivelDto>> GetDisponiveisAsync(long? idProduto, long? idCliente, CancellationToken ct);
    }
}