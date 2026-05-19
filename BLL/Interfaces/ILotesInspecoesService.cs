using GrupoTecnofix_Api.Dtos.Especificacao;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface ILotesInspecoesService
    {
        Task<List<LoteInspecaoDto>> GetByLoteAsync(int idLote, CancellationToken ct);
        Task<LoteInspecaoDto> GetByLoteEspecificacaoAsync(int idLote, int idEspecificacao, CancellationToken ct);
        Task<int> CreateOrUpdateAsync(LoteInspecaoDto dto, CancellationToken ct);
        Task CreateOrUpdateBatchAsync(int idLote, List<LoteInspecaoDto> items, string? statusLote, CancellationToken ct);
        Task DeleteAsync(int idLoteInspecao, CancellationToken ct);
    }
}
