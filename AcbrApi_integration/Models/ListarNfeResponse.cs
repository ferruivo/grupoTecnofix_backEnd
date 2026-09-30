using System.Text.Json.Serialization;

namespace AcbrApi_integration.Models
{
    public class ListarNfeResponse
    {
        [JsonPropertyName("data")]
        public List<NfeItemResponse> Data { get; set; } = new();
    }

    public class NfeItemResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("ambiente")]
        public string Ambiente { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("referencia")]
        public string Referencia { get; set; } = string.Empty;

        [JsonPropertyName("data_emissao")]
        public DateTime? DataEmissao { get; set; }

        [JsonPropertyName("modelo")]
        public int? Modelo { get; set; }

        [JsonPropertyName("serie")]
        public int? Serie { get; set; }

        [JsonPropertyName("numero")]
        public int? Numero { get; set; }

        [JsonPropertyName("tipo_emissao")]
        public int? TipoEmissao { get; set; }

        [JsonPropertyName("valor_total")]
        public decimal? ValorTotal { get; set; }

        [JsonPropertyName("chave")]
        public string? Chave { get; set; }

        [JsonPropertyName("autorizacao")]
        public NfeAutorizacaoResponse? Autorizacao { get; set; }
    }

    public class NfeAutorizacaoResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("ambiente")]
        public string Ambiente { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("autor")]
        public NfeAutorResponse? Autor { get; set; }

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

        [JsonPropertyName("digest_value")]
        public string? DigestValue { get; set; }
    }

    public class NfeAutorResponse
    {
        [JsonPropertyName("cpf_cnpj")]
        public string? CpfCnpj { get; set; }
    }
}
