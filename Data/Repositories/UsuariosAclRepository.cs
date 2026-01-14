using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class UsuariosAclRepository : IUsuariosAclRepository
    {
        private readonly AppDbContext _db;
        public UsuariosAclRepository(AppDbContext db) => _db = db;

        public Task<bool> UsuarioExistsAsync(int idUsuario, CancellationToken ct)
            => _db.Usuarios.AnyAsync(x => x.IdUsuario == idUsuario, ct);

        public async Task<bool> PerfisExistemAsync(List<int> idsPerfis, CancellationToken ct)
        {
            if (idsPerfis == null || idsPerfis.Count == 0) return true;

            var distinct = idsPerfis.Distinct().ToList();
            var count = await _db.Perfis.CountAsync(p => distinct.Contains(p.IdPerfil), ct);
            return count == distinct.Count;
        }

        public Task<List<int>> GetPerfisIdsAsync(int idUsuario, CancellationToken ct)
            => _db.UsuariosPerfis
                .Where(x => x.IdUsuario == idUsuario)
                .Select(x => x.IdPerfil)
                .ToListAsync(ct);

        public async Task ReplacePerfisAsync(int idUsuario, List<int> perfisIds, CancellationToken ct)
        {
            var atuais = _db.UsuariosPerfis.Where(x => x.IdUsuario == idUsuario);
            _db.UsuariosPerfis.RemoveRange(atuais);

            var distinct = (perfisIds ?? new List<int>()).Distinct().ToList();

            foreach (var idPerfil in distinct)
            {
                _db.UsuariosPerfis.Add(new UsuariosPerfi
                {
                    IdUsuario = idUsuario,
                    IdPerfil = idPerfil
                });
            }

            await Task.CompletedTask;
        }

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
