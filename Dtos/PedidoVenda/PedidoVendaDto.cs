using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;

namespace GrupoTecnofix_Api.Dtos.PedidoVenda
{
    public class PedidoVendaDto
    {
        public int IdPedidoVenda { get; set; }
        public int IdCliente { get; set; }
        public string? NomeCliente { get; set; }
        public int IdVendedor { get; set; }
        public string? NomeVendedor { get; set; }
        public int IdCondicaoPagamento { get; set; }
        public string? CondicaoPagamentoDescricao { get; set; }
        public CondicaoPagamentoDto? CondicaoPagamento { get; set; }
        public int? IdTransportadora { get; set; }
        public string? TransportadoraNome { get; set; }
        public DateTime DataPedido { get; set; }
        public string TipoFrete { get; set; } = "CIF"; // "CIF" ou "FOB"
        public decimal ValorFrete { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalIpi { get; set; }
        public decimal TotalIcms { get; set; }
        public decimal TotalPedido { get; set; }
        public string? Observacoes { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public List<PedidoVendaItemDto> Itens { get; set; } = new();
        public ClienteDto? Cliente { get; set; }
        public VendedorDto? Vendedor { get; set; }
    }
}