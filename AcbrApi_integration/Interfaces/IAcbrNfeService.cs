using AcbrApi_integration.Models;

namespace AcbrApi_integration.Interfaces
{
    public interface IAcbrNfeService
    {
        Task<byte[]> ObterPreviaPdfAsync(
            string empresaKey,
            EmitirNfeRequest request,
            bool logotipo = false,
            bool nomeFantasia = false,
            string formato = "padrao",
            string mensagemRodape = "",
            bool canhoto = true,
            CancellationToken cancellationToken = default);

        Task<EmitirNfeResponse> EmitirNfeAsync(
            string empresaKey,
            EmitirNfeRequest request,
            CancellationToken cancellationToken = default);

        Task<ListarNfeResponse> ListarNfeAsync(
            string empresaKey,
            ListarNfeRequest request,
            CancellationToken cancellationToken = default);

        Task<NfeItemResponse> ObterNfeAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default);

        Task<byte[]> ObterPdfAsync(
            string empresaKey,
            string idNfe,
            bool logotipo = false,
            bool nomeFantasia = false,
            string formato = "padrao",
            string mensagemRodape = "",
            bool canhoto = true,
            CancellationToken cancellationToken = default);

        Task<string> ObterXmlAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default);

        Task<CartaCorrecaoResponse> CartaCorrecaoAsync(
            string empresaKey,
            string idNfe,
            CartaCorrecaoRequest request,
            CancellationToken cancellationToken = default);

        Task<CartaCorrecaoResponse> ObterCartaCorrecaoAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default);

        Task<byte[]> ObterCartaCorrecaoPdfAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default);

        Task<CancelamentoNfeResponse> CancelarNfeAsync(
            string empresaKey,
            string idNfe,
            CancelamentoNfeRequest request,
            CancellationToken cancellationToken = default);

        Task<CancelamentoNfeResponse> ObterCancelamentoAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default);

        Task<byte[]> ObterCancelamentoPdfAsync(
            string empresaKey,
            string idNfe,
            CancellationToken cancellationToken = default);

        Task<ListarEventosNfeResponse> ListarEventosNfeAsync(
            string empresaKey,
            ListarEventosNfeRequest filtro,
            CancellationToken cancellationToken = default);
    }
}