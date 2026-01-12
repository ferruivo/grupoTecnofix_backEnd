using GrupoTecnofix_Api.Dtos.Auth;

namespace GrupoTecnofix_Api.Auth
{
    public interface IAuthService
    {
        Task<TokenResponse> LoginAsync(string login, string senha, HttpContext http);
        Task<TokenResponse> RefreshAsync(string refreshToken, HttpContext http);
        Task LogoutAsync(string refreshToken);
    }

}
