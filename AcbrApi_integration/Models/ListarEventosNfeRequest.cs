namespace AcbrApi_integration.Models
{
    public class ListarEventosNfeRequest
    {
        public int Top { get; set; } = 10;
        public int Skip { get; set; } = 0;
        public bool InlineCount { get; set; } = false;
        public string DfeId { get; set; } = string.Empty;
    }
}
