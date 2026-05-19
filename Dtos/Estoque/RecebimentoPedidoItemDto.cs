namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class RecebimentoPedidoItemDto
    {
        public int Id { get; set; }
        public int IdPedidoCompraItem { get; set; }
        public int IdPedidoCompra { get; set; }

        public int IdProduto { get; set; }
        public string ProdutoCodigo { get; set; } = string.Empty;
        public string ProdutoDescricao { get; set; } = string.Empty;

        public int Item { get; set; }

        public decimal Quantidade { get; set; }
        public decimal QuantidadeRecebida { get; set; }
        public decimal QuantidadeDisponivel { get; set; }

        public decimal PrecoUnitario { get; set; }

        public DateOnly? DataEntrega { get; set; }

        public bool Fechado { get; set; }
    }
}
