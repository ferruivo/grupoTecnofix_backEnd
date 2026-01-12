using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Dtos.Auth;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GrupoTecnofix_Api.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IPermissionService _perm;
        private readonly IConfiguration _cfg;

        public AuthService(AppDbContext db, IPermissionService perm, IConfiguration cfg)
        {
            _db = db;
            _perm = perm;
            _cfg = cfg;
        }

        public async Task<TokenResponse> LoginAsync(string login, string senha, HttpContext http)
        {
            var u = await _db.Usuarios.FirstOrDefaultAsync(x => x.Login == login && x.Ativo);
            if (u is null) throw new UnauthorizedAccessException("Login inválido");

            // BCrypt recomendado
            if (!BCrypt.Net.BCrypt.Verify(senha, u.SenhaHash))
                throw new UnauthorizedAccessException("Login inválido");

            return await IssueTokensAsync(u, http);
        }

        public async Task<TokenResponse> RefreshAsync(string refreshToken, HttpContext http)
        {
            var tokenHash = Sha256(refreshToken);

            var rt = await _db.TokensAtualizacao
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

            if (rt is null || rt.DataRevogacao != null || rt.DataExpiracao <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token inválido");

            var u = await _db.Usuarios.FirstAsync(x => x.IdUsuario == rt.IdUsuario && x.Ativo);

            // Rotaciona refresh token: revoga o antigo e cria outro
            rt.DataRevogacao = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return await IssueTokensAsync(u, http);
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var tokenHash = Sha256(refreshToken);
            var rt = await _db.TokensAtualizacao.FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
            if (rt != null && rt.DataRevogacao == null)
            {
                rt.DataRevogacao = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        private async Task<TokenResponse> IssueTokensAsync(Usuario u, HttpContext http)
        {
            var jwtCfg = _cfg.GetSection("Jwt");
            var issuer = jwtCfg["Issuer"]!;
            var audience = jwtCfg["Audience"]!;
            var key = jwtCfg["Key"]!;
            var accessMinutes = int.Parse(jwtCfg["AccessTokenMinutes"] ?? "60");
            var refreshDays = int.Parse(jwtCfg["RefreshTokenDays"] ?? "30");

            var perfis = await _perm.GetPerfisAsync(u.IdUsuario);
            var permissoes = await _perm.GetPermissoesAsync(u.IdUsuario);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, u.IdUsuario.ToString()),
            new Claim(ClaimTypes.NameIdentifier, u.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, u.Login),
            new Claim(ClaimTypes.Email, u.Email)
        };

            // Roles (perfis)
            foreach (var perfil in perfis)
                claims.Add(new Claim(ClaimTypes.Role, perfil));

            // Permissions
            foreach (var p in permissoes)
                claims.Add(new Claim("permission", p));

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256
            );

            var expires = DateTime.UtcNow.AddMinutes(accessMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // refresh token aleatório + salvar hash
            var refreshToken = GenerateSecureToken(64);
            var refreshHash = Sha256(refreshToken);

            var rt = new TokenAtualizacao
            {
                IdUsuario = u.IdUsuario,
                TokenHash = refreshHash,
                DataExpiracao = DateTime.UtcNow.AddDays(refreshDays),
                DataCadastro = DateTime.UtcNow,
                IpCriacao = http.Connection.RemoteIpAddress?.ToString(),
                UserAgentCriacao = http.Request.Headers.UserAgent.ToString()
            };

            _db.TokensAtualizacao.Add(rt);
            await _db.SaveChangesAsync();

            return new TokenResponse(accessToken, refreshToken, (int)TimeSpan.FromMinutes(accessMinutes).TotalSeconds);
        }

        private static string GenerateSecureToken(int bytes)
        {
            var data = RandomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(data);
        }

        private static string Sha256(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
