namespace GrupoTecnofix_Api.Dtos.Auth
{
    public record TokenResponseDto(string AccessToken, string RefreshToken, int ExpiresIn);
}
