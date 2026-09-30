using AcbrApi_integration.Models;

namespace AcbrApi_integration.Interfaces
{
    public interface IAcbrAuthService
    {
        Task<AcbrTokenResponse> AuthenticateAsync(
            string empresaKey,
            CancellationToken cancellationToken = default);

        Task<string> GetAccessTokenAsync(
            string empresaKey,
            CancellationToken cancellationToken = default);
    }
}