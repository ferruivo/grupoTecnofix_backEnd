using System.Text.Json.Serialization;

namespace AcbrApi_integration.Models
{
    public class CartaCorrecaoRequest
    {
        [JsonPropertyName("correcao")]
        public string Correcao { get; set; } = string.Empty;
    }
}
