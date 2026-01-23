using GrupoTecnofix_Api.Dtos.Produto;

namespace GrupoTecnofix_Api.Dtos.PedidoVenda
{
    public class PedidoVendaItemDto
    {
        public int IdPedidoVendaItem { get; set; }
        public int IdProduto { get; set; }
        public ProdutoDto? Produto { get; set; }
        public string? ProdutoCliente { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string? NossoPrazo { get; set; }
        public string? PrazoCliente { get; set; }
        public decimal? PesoItem { get; set; }
        public decimal? ValorKg { get; set; }
        public decimal AliquotaIpi { get; set; }
        public decimal AliquotaIcms { get; set; }
        public decimal TotalItem { get; set; }
        public decimal TotalIpi { get; set; }
        public decimal TotalIcms { get; set; }
        public DateTime? DataEntrega { get; set; }
    }
}