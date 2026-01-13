using GrupoTecnofix_Api.Dtos.Auth;

namespace GrupoTecnofix_Api.Auth
{
    public interface IAuthService
    {
        Task<TokenResponseDto> LoginAsync(string login, string senha, HttpContext http);
        Task<TokenResponseDto> RefreshAsync(string refreshToken, HttpContext http);
        Task LogoutAsync(string refreshToken);
    }

}
