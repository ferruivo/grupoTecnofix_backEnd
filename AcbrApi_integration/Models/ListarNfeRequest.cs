namespace AcbrApi_integration.Models
{
    public class ListarNfeRequest
    {
        public int Top { get; set; } = 10;

        public int Skip { get; set; } = 0;

        public bool InlineCount { get; set; } = false;

        public string? CpfCnpj { get; set; }

        public string Ambiente { get; set; } = "homologacao";
    }
}
