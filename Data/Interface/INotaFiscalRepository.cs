using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.NotaFiscal;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface INotaFiscalRepository
    {
        Task<PagedResult<NotaFiscalListDto>> GetListPagedAsync(
            int page,
            int pageSize,
            string? search,
            CancellationToken ct = default);

        Task<NotaFiscal?> GetByIdAsync(
            long idNotaFiscal,
            CancellationToken ct = default);

        Task<NotaFiscal?> GetByNumeroAsync(
            long numeroNota,
            int serie,
            string modelo,
            CancellationToken ct = default);

        Task<List<NotaFiscal>> GetAllAsync(
            CancellationToken ct = default);

        Task<NotaFiscal> AddAsync(
            NotaFiscal notaFiscal,
            CancellationToken ct = default);

        Task UpdateAsync(
            NotaFiscal notaFiscal,
            CancellationToken ct = default);

        Task DeleteAsync(
            long idNotaFiscal,
            CancellationToken ct = default);

        Task AddItemAsync(
            NotaFiscalItem item,
            CancellationToken ct = default);

        Task AddTributoItemAsync(
            NotaFiscalItemTributo tributo,
            CancellationToken ct = default);

        Task AddEventoAsync(
            NotaFiscalEvento evento,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            long idNotaFiscal,
            CancellationToken ct = default);

        Task<NotaFiscal?> GetByIdForUpdateAsync(long idNotaFiscal, CancellationToken ct = default);

        Task DeleteAsync(NotaFiscal notaFiscal, CancellationToken ct = default);

        Task SaveAsync(CancellationToken ct = default);
    }
}
