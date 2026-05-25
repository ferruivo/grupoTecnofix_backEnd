namespace GrupoTecnofix_Api.Dtos.NotaFiscal
{
    public class NotaFiscalPreviewRequestDto
    {
        public string TipoMovimento { get; set; } = "S";
        public string TipoOperacao { get; set; } = "VENDA";

        public int? IdDestinatario { get; set; }
        public string? TipoDestinatario { get; set; }

        public int IdNaturezaOperacao { get; set; }

        public decimal ValorFrete { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorDespesasAcessorias { get; set; }

        public List<NotaFiscalPreviewItemRequestDto> Itens { get; set; } = new();
    }

    public class NotaFiscalPreviewItemRequestDto
    {
        public string OrigemItem { get; set; } = string.Empty;
        // CONSUMO_LOTE, MANUAL, PLANILHA

        public int? IdConsumo { get; set; }
        public int? IdLote { get; set; }

        public int IdProduto { get; set; }

        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        public decimal ValorDesconto { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal ValorOutrasDespesas { get; set; }
        public decimal PercentualIcms { get; set; }
        public decimal PercentualIpi { get; set; }

        public string? Cfop { get; set; }
    }
}
