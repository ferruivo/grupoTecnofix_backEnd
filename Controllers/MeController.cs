using AutoMapper;
using GrupoTecnofix_Api.Auth;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly IPermissionService _perm;
        private readonly IUsuariosService _service;
        private readonly IMapper _mapper;

        private readonly AppDbContext _db;

        public MeController(IPermissionService perm, IUsuariosService service, IMapper mapper, AppDbContext db)
        {
            _perm = perm;
            _service = service;
            _mapper = mapper;
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

        [Authorize]
        [HttpPut("alterar-senha")]
        public async Task<IActionResult> AlterarSenha(UsuarioAtualizaSenhaDto dto)
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

            var u = await _service.GetByIdAsync(id, default);
            if (u is null) return NotFound(new { success = false, message = "Usuário não encontrado." });

            // ✅ Verifica senha atual corretamente (texto digitado vs hash do banco)
            if (!BCrypt.Net.BCrypt.Verify(dto.SenhaAtual, u.SenhaHash))
            {
                return BadRequest(new { success = false, message = "Senha atual inválida." });
            }

            // (opcional) impedir nova senha igual à atual
            if (BCrypt.Net.BCrypt.Verify(dto.NovaSenha, u.SenhaHash))
            {
                return Conflict(new { success = false, message = "A nova senha não pode ser igual à senha atual." });
            }

            // ✅ Gera novo hash e salva
            u.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);

            await _service.UpdateAsync(id, _mapper.Map<UsuarioUpdateDto>(u), default);

            return Ok(new { success = true, message = "Senha alterada com sucesso." });
        }
    }
}
