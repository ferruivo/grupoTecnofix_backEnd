using GrupoTecnofix_Api.Auth;
using GrupoTecnofix_Api.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = GrupoTecnofix_Api.Dtos.Auth.LoginRequest;
using RefreshRequest = GrupoTecnofix_Api.Dtos.Auth.RefreshRequest;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest req)
            => Ok(await _auth.LoginAsync(req.Login, req.Senha, HttpContext));

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenResponse>> Refresh([FromBody] RefreshRequest req)
            => Ok(await _auth.RefreshAsync(req.RefreshToken, HttpContext));

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshRequest req)
        {
            await _auth.LogoutAsync(req.RefreshToken);
            return NoContent();
        }
    }

}
