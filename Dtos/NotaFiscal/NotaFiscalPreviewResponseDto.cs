namespace GrupoTecnofix_Api.Dtos.NotaFiscal
{
    public class NotaFiscalPreviewResponseDto
    {
        public List<NotaFiscalPreviewItemResponseDto> Itens { get; set; } = new();

        public decimal ValorProdutos { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal ValorDespesasAcessorias { get; set; }

        public decimal BaseIcms { get; set; }
        public decimal ValorIcms { get; set; }
        public decimal BaseIcmsSt { get; set; }
        public decimal ValorIcmsSt { get; set; }

        public decimal ValorIpi { get; set; }
        public decimal ValorPis { get; set; }
        public decimal ValorCofins { get; set; }

        public decimal BaseCbs { get; set; }
        public decimal ValorCbs { get; set; }
        public decimal BaseIbsUf { get; set; }
        public decimal ValorIbsUf { get; set; }
        public decimal BaseIbsMunicipio { get; set; }
        public decimal ValorIbsMunicipio { get; set; }
        public decimal ValorIs { get; set; }

        public decimal ValorNota { get; set; }

        public List<string> Alertas { get; set; } = new();
    }

    public class NotaFiscalPreviewItemResponseDto
    {
        public int Item { get; set; }

        public string OrigemItem { get; set; } = string.Empty;

        public int IdProduto { get; set; }
        public string? CodigoProduto { get; set; }
        public string? DescricaoProduto { get; set; }
        public string? Ncm { get; set; }
        public string? Cfop { get; set; }

        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        public decimal PercentualIcms { get; set; }
        public decimal PercentualIpi { get; set; }

        public decimal ValorProduto { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorTotal { get; set; }


        public List<NotaFiscalPreviewTributoDto> Tributos { get; set; } = new();
    }

    public class NotaFiscalPreviewTributoDto
    {
        public string TipoTributo { get; set; } = string.Empty;
        public string? Cst { get; set; }
        public string? Csosn { get; set; }
        public string? CClassTrib { get; set; }

        public decimal BaseCalculo { get; set; }
        public decimal Aliquota { get; set; }
        public decimal Valor { get; set; }
    }
}
