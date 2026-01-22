namespace GrupoTecnofix_Api.Dtos.PedidoCompra
{
    public class PedidoCompraListItemDto
    {
        public int IdPedidoCompra { get; set; }
        public DateTime DataPedido { get; set; }
        public string FornecedorNome { get; set; } = string.Empty;
        public string CondicaoPagamentoDescricao { get; set; } = string.Empty;
        public decimal TotalPedido { get; set; }
    }

}
