using System.Text.Json.Serialization;

namespace AcbrApi_integration.Models
{
    public class CancelamentoNfeResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("ambiente")]
        public string Ambiente { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("autor")]
        public NfeEventoAutorResponse? Autor { get; set; }

        [JsonPropertyName("chave_acesso")]
        public string? ChaveAcesso { get; set; }

        [JsonPropertyName("data_evento")]
        public DateTime? DataEvento { get; set; }

        [JsonPropertyName("numero_sequencial")]
        public int? NumeroSequencial { get; set; }

        [JsonPropertyName("data_recebimento")]
        public DateTime? DataRecebimento { get; set; }

        [JsonPropertyName("codigo_status")]
        public int? CodigoStatus { get; set; }

        [JsonPropertyName("motivo_status")]
        public string? MotivoStatus { get; set; }

        [JsonPropertyName("numero_protocolo")]
        public string? NumeroProtocolo { get; set; }

        [JsonPropertyName("tipo_evento")]
        public string? TipoEvento { get; set; }

        [JsonPropertyName("justificativa")]
        public string? Justificativa { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object>? DadosExtras { get; set; }
    }

    public class NfeEventoAutorResponse
    {
        [JsonPropertyName("cpf_cnpj")]
        public string? CpfCnpj { get; set; }
    }
}
