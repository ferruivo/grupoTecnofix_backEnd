namespace GrupoTecnofix_Api.Dtos.PedidoCompra
{
    public class PedidoCompraItemDto
    {
        public int IdPedidoCompraItem { get; set; }
        public int IdPedidoCompra { get; set; }
        public int IdProduto { get; set; }
        public string? ProdutoCodigo { get; set; }
        public string? ProdutoDescricao { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal AliquotaIpi { get; set; }
        public decimal AliquotaIcms { get; set; }
        public decimal TotalItem { get; set; }
        public decimal TotalIpi { get; set; }
        public decimal TotalIcms { get; set; }
        public DateTime? DataEntrega { get; set; }
    }
}
