using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Fornecedor;

namespace GrupoTecnofix_Api.Dtos.PedidoCompra
{
    public class PedidoCompraDto
    {
        public int IdPedidoCompra { get; set; }
        public DateTime DataPedido { get; set; }
        public int IdFornecedor { get; set; }
        public string? FornecedorNome { get; set; }
        public int IdCondicaoPagamento { get; set; }
        public string? CondicaoPagamentoDescricao { get; set; }
        public int? IdTransportadora { get; set; }
        public string? TransportadoraNome { get; set; }
        public string TipoFrete { get; set; } = "CIF"; // "CIF" ou "FOB"
        public decimal ValorFrete { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalIpi { get; set; }
        public decimal TotalIcms { get; set; }
        public decimal TotalPedido { get; set; }
        public string? Observacao { get; set; }
        public string? ObservacaoComplementar { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public List<PedidoCompraItemDto> Itens { get; set; } = new();
        public FornecedorDto? Fornecedor { get; set; }
        public CondicaoPagamentoDto? CondicaoPagamento { get; set; }
    }
}
