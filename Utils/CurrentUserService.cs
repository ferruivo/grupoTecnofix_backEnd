
using System.Security.Claims;

namespace GrupoTecnofix_Api.Utils
{
    public interface ICurrentUserService
    {
        int GetUsuarioLogadoId();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public int GetUsuarioLogadoId()
        {
            var user = _httpContext.HttpContext?.User;

            if (user == null || user.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException();

            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? user.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(sub))
                throw new UnauthorizedAccessException("Token sem ID do usuário.");

            if (!int.TryParse(sub, out var idUsuario))
                throw new UnauthorizedAccessException("ID do usuário inválido no token.");

            return idUsuario;
        }
    }

}
