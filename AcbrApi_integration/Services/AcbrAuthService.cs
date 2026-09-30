using AcbrApi_integration.Interfaces;
using AcbrApi_integration.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AcbrApi_integration.Services
{
    public class AcbrAuthService : IAcbrAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AcbrApiOptions _options;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        // CACHE DE TOKENS POR EMPRESA
        private readonly Dictionary<string, AcbrTokenCache> _tokens = new();

        private class AcbrTokenCache
        {
            public AcbrTokenResponse Token { get; set; } = new();
            public DateTime ExpiresAtUtc { get; set; }
        }

        public AcbrAuthService(
            HttpClient httpClient,
            IOptions<AcbrApiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<AcbrTokenResponse> AuthenticateAsync(
            string empresaKey,
            CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                // BUSCA CONFIGURAÇÃO DA EMPRESA
                if (!_options.Empresas.TryGetValue(empresaKey, out var empresa))
                {
                    throw new Exception(
                        $"Empresa '{empresaKey}' não encontrada no appsettings.");
                }

                // VERIFICA CACHE
                if (_tokens.TryGetValue(empresaKey, out var cache))
                {
                    if (DateTime.UtcNow < cache.ExpiresAtUtc)
                    {
                        return cache.Token;
                    }
                }

                var form = new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = empresa.ClientId,
                    ["client_secret"] = empresa.ClientSecret,
                    ["scope"] = empresa.Scopes
                };

                using var content = new FormUrlEncodedContent(form);

                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                // DEFINE BASE URL DA EMPRESA
                _httpClient.BaseAddress = new Uri(empresa.AuthBaseUrl);

                var response = await _httpClient.PostAsync(
                    empresa.TokenEndpoint,
                    content,
                    cancellationToken);

                var responseBody = await response.Content.ReadAsStringAsync(
                    cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        $"Erro ao autenticar na ACBr API. " +
                        $"Status: {(int)response.StatusCode}. " +
                        $"Retorno: {responseBody}");
                }

                var token = JsonSerializer.Deserialize<AcbrTokenResponse>(
                    responseBody,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (token == null ||
                    string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    throw new Exception(
                        "Token inválido retornado pela ACBr API.");
                }

                // SALVA CACHE
                _tokens[empresaKey] = new AcbrTokenCache
                {
                    Token = token,

                    // margem de segurança de 5 minutos
                    ExpiresAtUtc = DateTime.UtcNow.AddSeconds(
                        token.ExpiresIn - 300)
                };

                return token;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<string> GetAccessTokenAsync(
            string empresaKey,
            CancellationToken cancellationToken = default)
        {
            var token = await AuthenticateAsync(
                empresaKey,
                cancellationToken);

            return token.AccessToken;
        }
    }
}