using System.Threading.Tasks;
using GrupoTecnofix_Api.Dtos.PedidoCompra;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IPdfService
    {
        Task<string> GeneratePdfBase64Async<T>(T model);
        Task<string> GeneratePurchaseOrderPdfBase64Async(PedidoCompraDto model);
    }
}
