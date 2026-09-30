using System.Text.Json.Serialization;

namespace AcbrApi_integration.Models
{
    public class EmitirNfeResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("ambiente")]
        public string Ambiente { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("referencia")]
        public string Referencia { get; set; } = string.Empty;

        [JsonPropertyName("chave")]
        public string? Chave { get; set; }

        [JsonPropertyName("numero")]
        public int? Numero { get; set; }

        [JsonPropertyName("serie")]
        public int? Serie { get; set; }

        [JsonPropertyName("valor_total")]
        public decimal? ValorTotal { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object>? DadosExtras { get; set; }

        [JsonPropertyName("http_status")]
        public int? HttpStatus { get; set; }

        [JsonPropertyName("is_error")]
        public bool IsError { get; set; }
    }
}
