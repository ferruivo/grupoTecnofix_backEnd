namespace GrupoTecnofix_Api.Dtos.NotaFiscal
{
    public class NotaFiscalListDto
    {
        public long IdNotaFiscal { get; set; }
        public long NumeroNota { get; set; }
        public int Serie { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public string? ChaveAcesso { get; set; }
        public DateOnly DataEmissao { get; set; }
        public string TipoMovimento { get; set; } = string.Empty;
        public string TipoOperacao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal ValorNota { get; set; }
        public int? IdDestinatario { get; set; }
    }
}
