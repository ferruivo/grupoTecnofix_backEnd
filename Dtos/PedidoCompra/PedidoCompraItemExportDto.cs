namespace GrupoTecnofix_Api.Dtos.PedidoCompra
{
    public class PedidoCompraItemExportDto
    {
        public string? ProdutoCodigo { get; set; }
        public string? ProdutoDescricao { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal TotalItem { get; set; }
    }
}
