using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface ILotesInspecoesRepository
    {
        Task<List<LoteInspecao>> GetByLoteAsync(int idLote, CancellationToken ct);
        Task<LoteInspecao?> GetByLoteEspecificacaoAsync(int idLote, int idEspecificacao, CancellationToken ct);
        Task AddAsync(LoteInspecao entity, CancellationToken ct);
        Task DeleteAsync(int idLoteInspecao, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
