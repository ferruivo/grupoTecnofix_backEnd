namespace GrupoTecnofix_Api.Dtos.NotaFiscal
{
    public class NotaFiscalItemTributoCreateDto
    {
        public string TipoTributo { get; set; } = string.Empty;
        // ICMS
        // ICMS_ST
        // IPI
        // PIS
        // COFINS
        // II
        // CBS
        // IBS_UF
        // IBS_MUN
        // IS

        public string? Cst { get; set; }

        public string? Csosn { get; set; }

        public string? CClassTrib { get; set; }

        public string? CodigoBeneficioFiscal { get; set; }

        public decimal BaseCalculo { get; set; }

        public decimal Aliquota { get; set; }

        public decimal Valor { get; set; }

        public decimal? PercentualReducao { get; set; }

        public decimal? ValorCreditoPresumido { get; set; }

        public string? Observacao { get; set; }
    }
}
