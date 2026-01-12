namespace GrupoTecnofix_Api.Dtos.Auth
{
    public record TokenResponse(string AccessToken, string RefreshToken, int ExpiresIn);
}
