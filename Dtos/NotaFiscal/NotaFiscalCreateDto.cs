namespace GrupoTecnofix_Api.Dtos.NotaFiscal
{
    public class NotaFiscalCreateDto
    {
        public long NumeroNota { get; set; }
        public int Serie { get; set; } = 1;
        public string Modelo { get; set; } = "55";

        public string TipoMovimento { get; set; } = string.Empty;
        public int Finalidade { get; set; }
        public string TipoOperacao { get; set; } = string.Empty;

        public DateOnly DataEmissao { get; set; }
        public DateTime? DataSaidaEntrada { get; set; }

        public int? IdDestinatario { get; set; }
        public string? TipoDestinatario { get; set; }

        public int? IdPagamento { get; set; }
        public int IdNaturezaOperacao { get; set; }

        public int? IdTransportadora { get; set; }

        public string? TipoFrete { get; set; }
        public string? Antt { get; set; }
        public string? Placa { get; set; }
        public string? UfPlaca { get; set; }

        public decimal PesoLiquido { get; set; }
        public decimal PesoBruto { get; set; }

        public decimal ValorFrete { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorDespesasAcessorias { get; set; }

        public string? Observacao { get; set; }
        public string? ObservacaoAdicional { get; set; }

        public string UsuarioEmissao { get; set; } = string.Empty;

        public List<NotaFiscalItemCreateDto> Itens { get; set; } = [];
    }

    public class NotaFiscalItemCreateDto
    {
        public int Item { get; set; }

        public int IdProduto { get; set; }

        public string? CodigoProduto { get; set; }

        public string? DescricaoProduto { get; set; }

        public string? Ncm { get; set; }

        public string? Cest { get; set; }

        public string? Cfop { get; set; }

        public string? Unidade { get; set; }

        public decimal Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; }

        public decimal ValorProduto { get; set; }

        public decimal ValorDesconto { get; set; }

        public decimal ValorFrete { get; set; }

        public decimal ValorSeguro { get; set; }

        public decimal ValorOutrasDespesas { get; set; }

        public decimal ValorTotal { get; set; }

        public string? PedidoCliente { get; set; }

        public string? ItemPedidoCliente { get; set; }

        public int? IdConsumo { get; set; }

        public List<NotaFiscalItemTributoCreateDto> Tributos { get; set; } = [];
    }
}
