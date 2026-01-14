using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class PerfisRepository : IPerfisRepository
    {
        private readonly AppDbContext _db;
        public PerfisRepository(AppDbContext db) => _db = db;

        public IQueryable<Perfi> Query() => _db.Perfis.AsQueryable();

        public Task<Perfi?> GetAsync(int id, CancellationToken ct)
            => _db.Perfis.FirstOrDefaultAsync(x => x.IdPerfil == id, ct);

        public Task<Perfi?> GetByNomeAsync(string nome, CancellationToken ct)
            => _db.Perfis.FirstOrDefaultAsync(x => x.Nome == nome, ct);

        public async Task AddAsync(Perfi entity, CancellationToken ct)
            => await _db.Perfis.AddAsync(entity, ct);

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);

        public Task<List<int>> GetPermissoesAsync(int idPerfil, CancellationToken ct)
    => _db.PerfisPermissoes
        .Where(x => x.IdPerfil == idPerfil)
        .Select(x => x.IdPermissao)
        .ToListAsync(ct);

        public async Task ReplacePermissoesAsync(int idPerfil, List<int> permissoesIds, CancellationToken ct)
        {
            var atuais = _db.PerfisPermissoes.Where(x => x.IdPerfil == idPerfil);
            _db.PerfisPermissoes.RemoveRange(atuais);

            var distinct = (permissoesIds ?? new List<int>())
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            foreach (var idPermissao in distinct)
            {
                _db.PerfisPermissoes.Add(new PerfisPermisso
                {
                    IdPerfil = idPerfil,
                    IdPermissao = idPermissao,
                    DataCadastro = DateTime.Now
                });
            }

            await Task.CompletedTask;
        }

        public async Task<bool> PermissoesExistemAsync(List<int> idsPermissao, CancellationToken ct)
        {
            if (idsPermissao == null || idsPermissao.Count == 0) return true;

            var distinct = idsPermissao.Where(x => x > 0).Distinct().ToList();
            var count = await _db.Permissoes.CountAsync(p => distinct.Contains(p.IdPermissao), ct);
            return count == distinct.Count;
        }
    }
}
