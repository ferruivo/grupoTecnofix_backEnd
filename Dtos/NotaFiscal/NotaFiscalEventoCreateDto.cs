namespace GrupoTecnofix_Api.Dtos.NotaFiscal
{
    public class NotaFiscalEventoCreateDto
    {
        public long IdNotaFiscal { get; set; }

        public string TipoEvento { get; set; } = string.Empty;

        public string? Protocolo { get; set; }

        public string? Usuario { get; set; }

        public string? Justificativa { get; set; }

        public string? XmlRetorno { get; set; }
    }
}
