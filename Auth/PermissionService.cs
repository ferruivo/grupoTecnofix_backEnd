using GrupoTecnofix_Api.Data;
using Microsoft.EntityFrameworkCore;


namespace GrupoTecnofix_Api.Auth
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _db;
        public PermissionService(AppDbContext db) => _db = db;

        public async Task<List<string>> GetPerfisAsync(int idUsuario)
        {
            return await (from up in _db.UsuariosPerfis
                          join p in _db.Perfis on up.IdPerfil equals p.IdPerfil
                          where up.IdUsuario == idUsuario
                          select p.Nome).Distinct().ToListAsync();
        }

        public async Task<List<string>> GetPermissoesAsync(int idUsuario)
        {
            // permissões via perfil
            var viaPerfil = await (from up in _db.UsuariosPerfis
                                   join pp in _db.PerfisPermissoes on up.IdPerfil equals pp.IdPerfil
                                   join pm in _db.Permissoes on pp.IdPermissao equals pm.IdPermissao
                                   where up.IdUsuario == idUsuario
                                   select pm.Codigo).Distinct().ToListAsync();

            // opcional: permissões diretas no usuário (se você usar USUARIOS_PERMISSOES)
            // var diretas = ...

            return viaPerfil;
        }
    }
}
