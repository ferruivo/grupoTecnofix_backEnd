namespace GrupoTecnofix_Api.Dtos.PedidoVenda
{
    public class PedidoVendaCreateUpdateDto
    {
        public int IdCliente { get; set; }
        public int IdVendedor { get; set; }
        public int IdCondicaoPagamento { get; set; }
        public int? IdTransportadora { get; set; }
        public DateTime DataPedido { get; set; }
        public string TipoFrete { get; set; } = "CIF";
        public decimal ValorFrete { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalIpi { get; set; }
        public decimal TotalIcms { get; set; }
        public decimal TotalPedido { get; set; }
        public string? Observacoes { get; set; }

        public List<PedidoVendaItemCreateUpdateDto> Itens { get; set; } = new();
    }
}