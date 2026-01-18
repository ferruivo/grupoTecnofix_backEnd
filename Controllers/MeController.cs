using GrupoTecnofix_Api.Auth;
using GrupoTecnofix_Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly IPermissionService _perm;
        private readonly AppDbContext _db;

        public MeController(IPermissionService perm, AppDbContext db)
        {
            _perm = perm;
            _db = db;
        }

        [Authorize]
        [HttpGet("/me")]
        public async Task<IActionResult> Me()
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

            var u = await _db.Usuarios.FindAsync(id);
            if (u is null) return NotFound();

            var perfis = await _perm.GetPerfisAsync(id);
            var permissoes = await _perm.GetPermissoesAsync(id);

            return Ok(new
            {
                idUsuario = u.IdUsuario,
                nome = u.NomeCompleto,
                nomeExibicao = u.NomeExibicao,
                login = u.Login,
                email = u.Email,
                perfis,
                permissoes
            });
        }
    }
}
