using AcbrApi_integration.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcbrApi_integration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAcbrAuthService _acbrAuthService;

        public AuthController(IAcbrAuthService acbrAuthService)
        {
            _acbrAuthService = acbrAuthService;
        }

        [HttpPost("{empresaKey}/token")]
        public async Task<IActionResult> Authenticate(
            string empresaKey,
            CancellationToken cancellationToken)
        {
            var token = await _acbrAuthService.AuthenticateAsync(
                empresaKey,
                cancellationToken);

            return Ok(token);
        }
    }
}