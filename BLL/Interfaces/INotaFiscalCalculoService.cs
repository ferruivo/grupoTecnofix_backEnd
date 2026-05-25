using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface INotaFiscalCalculoService
    {
        Task CalcularAsync(NotaFiscal nota, CancellationToken ct = default);
    }
}
