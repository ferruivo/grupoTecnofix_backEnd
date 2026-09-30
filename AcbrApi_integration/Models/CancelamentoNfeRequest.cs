using System.Text.Json.Serialization;

namespace AcbrApi_integration.Models
{
    public class CancelamentoNfeRequest
    {
        [JsonPropertyName("justificativa")]
        public string Justificativa { get; set; } = string.Empty;
    }
}
