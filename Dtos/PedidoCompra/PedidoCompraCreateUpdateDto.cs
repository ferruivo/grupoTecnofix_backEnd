namespace GrupoTecnofix_Api.Dtos.PedidoCompra
{
    public class PedidoCompraCreateUpdateDto
    {
        public DateTime DataPedido { get; set; }
        public int IdFornecedor { get; set; }
        public int IdCondicaoPagamento { get; set; }
        public int? IdTransportadora { get; set; }
        public string TipoFrete { get; set; } = "CIF";
        public decimal ValorFrete { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalIpi { get; set; }
        public decimal TotalIcms { get; set; }
        public decimal TotalPedido { get; set; }
        public string? Observacao { get; set; }
        public string? ObservacaoComplementar { get; set; }

        public List<PedidoCompraItemCreateUpdateDto> Itens { get; set; } = new();
    }
}
