using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class PermissoesRepository : IPermissoesRepository
    {
        private readonly AppDbContext _db;
        public PermissoesRepository(AppDbContext db) => _db = db;

        public IQueryable<Permisso> Query() => _db.Permissoes.AsQueryable();

        public Task<Permisso?> GetAsync(int idPermissao, CancellationToken ct)
            => _db.Permissoes.FirstOrDefaultAsync(x => x.IdPermissao == idPermissao, ct);

        public Task<bool> ExistsAsync(int idPermissao, CancellationToken ct)
            => _db.Permissoes.AnyAsync(x => x.IdPermissao == idPermissao, ct);
    }
}
