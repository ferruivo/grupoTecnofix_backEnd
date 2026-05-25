namespace GrupoTecnofix_Api.Dtos.NotaFiscal
{
    public class NotaFiscalUpdateDto
    {
        public long IdNotaFiscal { get; set; }

        public string? ChaveAcesso { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? ProtocoloAutorizacao { get; set; }

        public DateTime? DataAutorizacao { get; set; }

        public decimal ValorFrete { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorDespesasAcessorias { get; set; }

        public string? Observacao { get; set; }
        public string? ObservacaoAdicional { get; set; }

        public List<NotaFiscalItemUpdateDto> Itens { get; set; } = [];
    }

    public class NotaFiscalItemUpdateDto
    {
        public long IdNotaFiscalItem { get; set; }

        public decimal Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; }

        public decimal ValorProduto { get; set; }

        public decimal ValorDesconto { get; set; }

        public decimal ValorFrete { get; set; }

        public decimal ValorSeguro { get; set; }

        public decimal ValorOutrasDespesas { get; set; }

        public decimal ValorTotal { get; set; }

        public List<NotaFiscalItemTributoCreateDto> Tributos { get; set; } = [];
    }
}
