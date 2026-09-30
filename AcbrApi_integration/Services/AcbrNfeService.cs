using AcbrApi_integration.Interfaces;
using AcbrApi_integration.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AcbrApi_integration.Services
{
    public class AcbrNfeService : IAcbrNfeService
    {
        private readonly HttpClient _httpClient;
        private readonly IAcbrAuthService _authService;
        private readonly AcbrApiOptions _options;

        public AcbrNfeService(
            HttpClient httpClient,
            IAcbrAuthService authService,
            IOptions<AcbrApiOptions> options)
        {
            _httpClient = httpClient;
            _authService = authService;
            _options = options.Value;
        }

        private void SetEmpresaBaseAddress(string empresaKey)
        {
            if (!_options.Empresas.TryGetValue(empresaKey, out var empresa))
                throw new Exception($"Empresa '{empresaKey}' não encontrada no appsettings.");

            _httpClient.BaseAddress = new Uri(empresa.ApiBaseUrl);
        }

        public async Task<byte[]> ObterPreviaPdfAsync(
            string empresaKey,
            EmitirNfeRequest request,
            bool logotipo = false,
            bool nomeFantasia = false,
            string formato = "padrao",
            string mensagemRodape = "",
            bool canhoto = true,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var token = await _authService.GetAccessTokenAsync(empresaKey, cancellationToken);

            var url = QueryHelpers.AddQueryString("/nfe/previa/pdf", new Dictionary<string, string?>
            {
                ["logotipo"] = logotipo.ToString().ToLower(),
                ["nome_fantasia"] = nomeFantasia.ToString().ToLower(),
                ["formato"] = formato,
                ["mensagem_rodape"] = mensagemRodape,
                ["canhoto"] = canhoto.ToString().ToLower()
            });

            var response = await SendJsonWithRetryAsync(
                empresaKey,
                HttpMethod.Post,
                url,
                request,
                "*/*",
                cancellationToken);

            var pdfBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var erro = System.Text.Encoding.UTF8.GetString(pdfBytes);
                throw new Exception($"Erro ao gerar pré-visualização do PDF da NFe. Status: {(int)response.StatusCode}. Retorno: {erro}");
            }

            return pdfBytes;
        }

        public async Task<EmitirNfeResponse> EmitirNfeAsync(
            string empresaKey,
            EmitirNfeRequest request,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var response = await SendJsonWithRetryAsync(
                empresaKey,
                HttpMethod.Post,
                "/nfe",
                request,
                "application/json",
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            // Do not throw for non-success here. Return a structured EmitirNfeResponse
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var ok = JsonSerializer.Deserialize<EmitirNfeResponse>(responseBody, jsonOptions);
                    if (ok != null)
                        return ok;
                }
                catch
                {
                    // fallthrough and return raw content in DadosExtras
                }

                return new EmitirNfeResponse
                {
                    DadosExtras = new Dictionary<string, object>
                    {
                        ["http_status"] = (int)response.StatusCode,
                        ["raw"] = responseBody
                    }
                };
            }

            // Non-success: attempt to extract error object and return inside DadosExtras
            try
            {
                var extras = new Dictionary<string, object>
                {
                    ["http_status"] = (int)response.StatusCode,
                    ["raw"] = responseBody
                };

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("error", out var err))
                {
                    extras["error"] = JsonSerializer.Deserialize<object>(err.GetRawText(), jsonOptions) ?? err.GetRawText();
                }
                else
                {
                    extras["error"] = JsonSerializer.Deserialize<object>(responseBody, jsonOptions) ?? responseBody;
                }

                return new EmitirNfeResponse
                {
                    DadosExtras = extras
                };
            }
            catch
            {
                return new EmitirNfeResponse
                {
                    DadosExtras = new Dictionary<string, object>
                    {
                        ["http_status"] = (int)response.StatusCode,
                        ["raw"] = responseBody
                    }
                };
            }
        }

        public async Task<NfeItemResponse> ObterNfeAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var response = await SendWithRetryAsync(
                empresaKey,
                HttpMethod.Get,
                $"/nfe/{idNfe}",
                "application/json",
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao consultar NFe. Status: {(int)response.StatusCode}. Retorno: {responseBody}");

            return DeserializeOrThrow<NfeItemResponse>(responseBody, "Resposta inválida da ACBr API.");
        }

        public async Task<ListarNfeResponse> ListarNfeAsync(
            string empresaKey,
            ListarNfeRequest filtro,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var queryParams = new Dictionary<string, string?>
            {
                ["$top"] = filtro.Top.ToString(),
                ["$skip"] = filtro.Skip.ToString(),
                ["$inlinecount"] = filtro.InlineCount.ToString().ToLower(),
                ["cpf_cnpj"] = filtro.CpfCnpj,
                ["ambiente"] = filtro.Ambiente
            };

            var url = QueryHelpers.AddQueryString("/nfe", queryParams);

            var response = await SendWithRetryAsync(
                empresaKey,
                HttpMethod.Get,
                url,
                "application/json",
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao listar NFe. Status: {(int)response.StatusCode}. Retorno: {responseBody}");

            return DeserializeOrThrow<ListarNfeResponse>(responseBody, "Resposta inválida da ACBr API.");
        }

        public async Task<byte[]> ObterPdfAsync(
            string empresaKey,
            string idNfe,
            bool logotipo = false,
            bool nomeFantasia = false,
            string formato = "padrao",
            string mensagemRodape = "",
            bool canhoto = true,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var url = QueryHelpers.AddQueryString($"/nfe/{idNfe}/pdf", new Dictionary<string, string?>
            {
                ["logotipo"] = logotipo.ToString().ToLower(),
                ["nome_fantasia"] = nomeFantasia.ToString().ToLower(),
                ["formato"] = formato,
                ["mensagem_rodape"] = mensagemRodape,
                ["canhoto"] = canhoto.ToString().ToLower()
            });

            var response = await SendWithRetryAsync(
                empresaKey,
                HttpMethod.Get,
                url,
                "*/*",
                cancellationToken);

            var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var erro = System.Text.Encoding.UTF8.GetString(content);
                throw new Exception($"Erro ao obter PDF da NFe. Status: {(int)response.StatusCode}. Retorno: {erro}");
            }

            return content;
        }

        public async Task<string> ObterXmlAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var response = await SendWithRetryAsync(
                empresaKey,
                HttpMethod.Get,
                $"/nfe/{idNfe}/xml/nota",
                "*/*",
                cancellationToken);

            var xml = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao obter XML da NFe. Status: {(int)response.StatusCode}. Retorno: {xml}");

            return xml;
        }

        public async Task<CartaCorrecaoResponse> CartaCorrecaoAsync(
            string empresaKey,
            string idNfe,
            CartaCorrecaoRequest request,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var response = await SendJsonWithRetryAsync(
                empresaKey,
                HttpMethod.Post,
                $"/nfe/{idNfe}/carta-correcao",
                request,
                "application/json",
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao emitir Carta de Correção. Status: {(int)response.StatusCode}. Retorno: {responseBody}");

            return DeserializeOrThrow<CartaCorrecaoResponse>(responseBody, "Resposta inválida da ACBr API.");
        }

        public async Task<CartaCorrecaoResponse> ObterCartaCorrecaoAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var response = await SendWithRetryAsync(
                empresaKey,
                HttpMethod.Get,
                $"/nfe/{idNfe}/carta-correcao",
                "application/json",
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao obter Carta de Correção. Status: {(int)response.StatusCode}. Retorno: {responseBody}");

            return DeserializeOrThrow<CartaCorrecaoResponse>(responseBody, "Resposta inválida da ACBr API.");
        }

        public async Task<byte[]> ObterCartaCorrecaoPdfAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var response = await SendWithRetryAsync(
                empresaKey,
                HttpMethod.Get,
                $"/nfe/{idNfe}/carta-correcao/pdf",
                "*/*",
                cancellationToken);

            var pdfBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var erro = System.Text.Encoding.UTF8.GetString(pdfBytes);
                throw new Exception($"Erro ao obter PDF da Carta de Correção. Status: {(int)response.StatusCode}. Retorno: {erro}");
            }

            return pdfBytes;
        }

        public async Task<CancelamentoNfeResponse> CancelarNfeAsync(
            string empresaKey,
            string idNfe,
            CancelamentoNfeRequest request,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var response = await SendJsonWithRetryAsync(
                empresaKey,
                HttpMethod.Post,
                $"/nfe/{idNfe}/cancelamento",
                request,
                "application/json",
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var ok = JsonSerializer.Deserialize<CancelamentoNfeResponse>(responseBody, jsonOptions);
                    if (ok != null)
                        return ok;
                }
                catch
                {
                    // fallthrough and return raw content in DadosExtras
                }

                return new CancelamentoNfeResponse
                {
                    DadosExtras = new Dictionary<string, object>
                    {
                        ["http_status"] = (int)response.StatusCode,
                        ["raw"] = responseBody
                    }
                };
            }

            // Non-success: attempt to extract error object and return inside DadosExtras
            try
            {
                var extras = new Dictionary<string, object>
                {
                    ["http_status"] = (int)response.StatusCode,
                    ["raw"] = responseBody
                };

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("error", out var err))
                {
                    extras["error"] = JsonSerializer.Deserialize<object>(err.GetRawText(), jsonOptions) ?? err.GetRawText();
                }
                else
                {
                    extras["error"] = JsonSerializer.Deserialize<object>(responseBody, jsonOptions) ?? responseBody;
                }

                return new CancelamentoNfeResponse
                {
                    DadosExtras = extras
                };
            }
            catch
            {
                return new CancelamentoNfeResponse
                {
                    DadosExtras = new Dictionary<string, object>
                    {
                        ["http_status"] = (int)response.StatusCode,
                        ["raw"] = responseBody
                    }
                };
            }
        }

        public async Task<CancelamentoNfeResponse> ObterCancelamentoAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var response = await SendWithRetryAsync(
                empresaKey,
                HttpMethod.Get,
                $"/nfe/{idNfe}/cancelamento",
                "application/json",
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao obter cancelamento da NFe. Status: {(int)response.StatusCode}. Retorno: {responseBody}");

            return DeserializeOrThrow<CancelamentoNfeResponse>(responseBody, "Resposta inválida da ACBr API.");
        }

        public async Task<byte[]> ObterCancelamentoPdfAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var response = await SendWithRetryAsync(
                empresaKey,
                HttpMethod.Get,
                $"/nfe/{idNfe}/cancelamento/pdf",
                "*/*",
                cancellationToken);

            var pdfBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var erro = System.Text.Encoding.UTF8.GetString(pdfBytes);
                throw new Exception($"Erro ao obter PDF do cancelamento. Status: {(int)response.StatusCode}. Retorno: {erro}");
            }

            return pdfBytes;
        }

        public async Task<ListarEventosNfeResponse> ListarEventosNfeAsync(
            string empresaKey,
            ListarEventosNfeRequest filtro,
            CancellationToken cancellationToken = default)
        {
            SetEmpresaBaseAddress(empresaKey);

            var queryParams = new Dictionary<string, string?>
            {
                ["$top"] = filtro.Top.ToString(),
                ["$skip"] = filtro.Skip.ToString(),
                ["$inlinecount"] = filtro.InlineCount.ToString().ToLower(),
                ["dfe_id"] = filtro.DfeId
            };

            var url = QueryHelpers.AddQueryString("/nfe/eventos", queryParams);

            var response = await SendWithRetryAsync(
                empresaKey,
                HttpMethod.Get,
                url,
                "application/json",
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao listar eventos da NFe. Status: {(int)response.StatusCode}. Retorno: {responseBody}");

            return DeserializeOrThrow<ListarEventosNfeResponse>(responseBody, "Resposta inválida da ACBr API.");
        }

        private async Task<HttpResponseMessage> SendWithRetryAsync(
            string empresaKey,
            HttpMethod method,
            string url,
            string accept,
            CancellationToken cancellationToken)
        {
            var token = await _authService.GetAccessTokenAsync(empresaKey, cancellationToken);

            using var request = new HttpRequestMessage(method, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            response.Dispose();

            token = await _authService.AuthenticateAsync(empresaKey, cancellationToken)
                .ContinueWith(t => t.Result.AccessToken, cancellationToken);

            using var retryRequest = new HttpRequestMessage(method, url);
            retryRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(retryRequest, cancellationToken);
        }

        private async Task<HttpResponseMessage> SendJsonWithRetryAsync<TRequest>(
            string empresaKey,
            HttpMethod method,
            string url,
            TRequest body,
            string accept,
            CancellationToken cancellationToken)
        {
            var token = await _authService.GetAccessTokenAsync(empresaKey, cancellationToken);

            using var request = new HttpRequestMessage(method, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(
                body,
                options: new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            response.Dispose();

            token = await _authService.AuthenticateAsync(empresaKey, cancellationToken)
                .ContinueWith(t => t.Result.AccessToken, cancellationToken);

            using var retryRequest = new HttpRequestMessage(method, url);
            retryRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            retryRequest.Content = JsonContent.Create(
                body,
                options: new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

            return await _httpClient.SendAsync(retryRequest, cancellationToken);
        }

        private static T DeserializeOrThrow<T>(
            string json,
            string errorMessage)
        {
            var result = JsonSerializer.Deserialize<T>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result == null)
                throw new Exception(errorMessage);

            return result;
        }
    }
}