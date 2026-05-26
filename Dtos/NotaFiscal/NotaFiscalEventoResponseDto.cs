namespace GrupoTecnofix_Api.Dtos.NotaFiscal
{
    public class NotaFiscalEventoResponseDto
    {
        public long IdNotaFiscalEvento { get; set; }
        public DateTime DataEvento { get; set; }
        public string TipoEvento { get; set; } = string.Empty;
        public string? Usuario { get; set; }
        public string? XmlRetorno { get; set; }

        // Human readable fields
        public string? Summary { get; set; }
        public List<string>? Errors { get; set; }
        public Dictionary<string, string?>? Details { get; set; }
    }
}
