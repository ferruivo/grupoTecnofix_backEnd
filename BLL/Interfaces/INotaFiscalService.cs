using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.NotaFiscal;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface INotaFiscalService
    {
        Task<PagedResult<NotaFiscalListDto>> GetPagedAsync(
            int page,
            int pageSize,
            string? search,
            CancellationToken ct);

        Task<NotaFiscal?> GetByIdAsync(
            long idNotaFiscal,
            CancellationToken ct);

        Task<long> CreateAsync(
            NotaFiscalCreateDto dto,
            CancellationToken ct);

        Task UpdateAsync(
            long idNotaFiscal,
            NotaFiscalUpdateDto dto,
            CancellationToken ct);

        Task DeleteAsync(
            long idNotaFiscal,
            CancellationToken ct);

        Task AddEventoAsync(
            NotaFiscalEventoCreateDto dto,
            CancellationToken ct);

        Task EmitirAsync(
    long idNotaFiscal,
    CancellationToken ct);

        Task<List<GrupoTecnofix_Api.Dtos.NotaFiscal.NotaFiscalEventoResponseDto>> GetEventosAsync(long idNotaFiscal, CancellationToken ct);

        Task<NotaFiscalImportacaoPreviewDto> ImportarItensPreviewAsync(
            IFormFile arquivo,
            CancellationToken ct);
    }
}
