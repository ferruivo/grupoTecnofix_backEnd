using GrupoTecnofix_Api.Auth;
using GrupoTecnofix_Api.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using LoginRequestDto = GrupoTecnofix_Api.Dtos.Auth.LoginRequestDto;
using RefreshRequestDto = GrupoTecnofix_Api.Dtos.Auth.RefreshRequestDto;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginRequestDto req)
            => Ok(await _auth.LoginAsync(req.Login, req.Senha, HttpContext));

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenResponseDto>> Refresh([FromBody] RefreshRequestDto req)
            => Ok(await _auth.RefreshAsync(req.RefreshToken, HttpContext));

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshRequestDto req)
        {
            await _auth.LogoutAsync(req.RefreshToken);
            return NoContent();
        }
    }

}
