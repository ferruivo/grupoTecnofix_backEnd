using GrupoTecnofix_Api.Dtos.NotaFiscal;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface INotaFiscalPreviewService
    {
        Task<NotaFiscalPreviewResponseDto> GerarPreviewAsync(
            NotaFiscalPreviewRequestDto dto,
            CancellationToken ct);
    }
}
