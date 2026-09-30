namespace AcbrApi_integration
{
    public class AcbrApiOptions
    {
        public Dictionary<string, AcbrEmpresaOptions> Empresas { get; set; } = new();
    }

    public class AcbrEmpresaOptions
    {
        public string AuthBaseUrl { get; set; } = string.Empty;

        public string ApiBaseUrl { get; set; } = string.Empty;

        public string TokenEndpoint { get; set; } = string.Empty;

        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;

        public string Scopes { get; set; } = string.Empty;
    }
}
