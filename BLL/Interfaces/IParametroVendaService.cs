using GrupoTecnofix_Api.Dtos.ParametroVenda;
using GrupoTecnofix_Api.Dtos.PedidoCompra;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IParametroVendaService
    {
        Task<ParametroVendaDto?> GetAsync(long idProduto, long? idCliente, CancellationToken ct);
    }
}
