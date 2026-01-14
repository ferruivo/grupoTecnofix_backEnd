using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Perfis;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly AppDbContext _db;

        public UsuariosRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<UsuarioListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _db.Usuarios.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(u =>
                    u.NomeCompleto.Contains(s) ||
                    u.Login.Contains(s) ||
                    u.Email.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(u => u.NomeCompleto)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UsuarioListDto
                {
                    IdUsuario = u.IdUsuario,
                    NomeCompleto = u.NomeCompleto,
                    Login = u.Login,
                    Email = u.Email,
                    Ativo = u.Ativo,

                    Perfis = (
                        from up in _db.UsuariosPerfis
                        join p in _db.Perfis on up.IdPerfil equals p.IdPerfil
                        where up.IdUsuario == u.IdUsuario
                        select new PerfilDto
                        {
                            Nome = p.Nome
                        }
                    ).ToList()
                })
                .ToListAsync(ct);

            return new PagedResult<UsuarioListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<List<UsuarioListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            var query = _db.Usuarios.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(u =>
                    u.NomeCompleto.Contains(s) ||
                    u.Login.Contains(s) ||
                    u.Email.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(u => u.NomeCompleto)
                .Select(u => new UsuarioListDto
                {
                    IdUsuario = u.IdUsuario,
                    NomeCompleto = u.NomeCompleto,
                    Login = u.Login,
                    Email = u.Email,
                    Ativo = u.Ativo,

                    Perfis = (
                        from up in _db.UsuariosPerfis
                        join p in _db.Perfis on up.IdPerfil equals p.IdPerfil
                        where up.IdUsuario == u.IdUsuario
                        select new PerfilDto
                        {
                            Nome = p.Nome
                        }
                    ).ToList()
                })
                .ToListAsync(ct);
           
            return items;

        
        }
        public Task<Usuario?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id, ct);

        public async Task AddAsync(Usuario usuario, CancellationToken ct)
            => await _db.Usuarios.AddAsync(usuario, ct);

        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
