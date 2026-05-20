namespace GrupoTecnofix_Api.Dtos.PedidoVenda
{
    public class PedidoVendaPendenteItemDto
    {
        public int IdPedidoVenda { get; set; }
        public int NumeroPedido { get; set; }
        public string? Item { get; set; }

        public int IdProduto { get; set; }
        public string ProdutoCodigo { get; set; } = string.Empty;
        public string ProdutoDescricao { get; set; } = string.Empty;

        public decimal Quantidade { get; set; }
        public decimal Disponivel { get; set; }
        public decimal Liberado { get; set; }

        public string? ProdutoCliente { get; set; }
        public string? NossoPrazo { get; set; }
        public string? PrazoCliente { get; set; }
    }
}
