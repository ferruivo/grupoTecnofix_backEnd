using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IConsumoLoteRepository
    {
        Task<List<ConsumoloteDto>> GetListAsync(long? idConsumolote, long? idpedidovenda,long? item, long? idlote, CancellationToken ct);
        Task<Consumolote?> GetByIdAsync(int id, CancellationToken ct);
        Task<long?> GetSumAsync(long? idpedidovenda, long? item, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
