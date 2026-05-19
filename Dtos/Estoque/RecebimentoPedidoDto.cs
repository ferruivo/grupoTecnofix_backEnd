namespace GrupoTecnofix_Api.Dtos.Estoque
{
    public class RecebimentoPedidoDto
    {
        public int IdPedidoCompra { get; set; }
        public int Numero { get; set; }
        public DateOnly DataPedido { get; set; }

        public int IdFornecedor { get; set; }
        public string FornecedorNome { get; set; } = string.Empty;

        public int IdCondicaoPagamento { get; set; }
        public string CondicaoPagamentoDescricao { get; set; } = string.Empty;

        public string TipoFrete { get; set; } = string.Empty;

        public decimal TotalIpi { get; set; }
        public decimal TotalIcms { get; set; }

        public string? Observacao { get; set; }

        public bool Fechado { get; set; }

        public List<RecebimentoPedidoItemDto> Itens { get; set; } = [];
    }
}
