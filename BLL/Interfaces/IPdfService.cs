using System.Threading.Tasks;
using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.PedidoCompra;
using GrupoTecnofix_Api.Dtos.Usuario;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IPdfService
    {
        Task<string> GeneratePdfBase64Async<T>(T model);
        Task<string> GeneratePurchaseOrderPdfBase64Async(EmpresaDto empresa,UsuarioDto usuario, PedidoCompraDto model);
    }
}
