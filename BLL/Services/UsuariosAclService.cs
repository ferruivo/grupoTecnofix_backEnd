using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class UsuariosAclService : IUsuariosAclService
    {
        private readonly IUsuariosAclRepository _repo;

        public UsuariosAclService(IUsuariosAclRepository repo) => _repo = repo;

        public async Task<List<int>> GetPerfisIdsAsync(int idUsuario, CancellationToken ct)
        {
            var exists = await _repo.UsuarioExistsAsync(idUsuario, ct);
            if (!exists) throw new KeyNotFoundException("Usuário não encontrado.");

            return await _repo.GetPerfisIdsAsync(idUsuario, ct);
        }

        public async Task UpdatePerfisAsync(int idUsuario, List<int> perfisIds, CancellationToken ct)
        {
            var exists = await _repo.UsuarioExistsAsync(idUsuario, ct);
            if (!exists) throw new KeyNotFoundException("Usuário não encontrado.");

            var ids = (perfisIds ?? new List<int>()).Distinct().ToList();

            var perfisOk = await _repo.PerfisExistemAsync(ids, ct);
            if (!perfisOk) throw new InvalidOperationException("Um ou mais perfis informados não existem.");

            await _repo.ReplacePerfisAsync(idUsuario, ids, ct);
            await _repo.SaveAsync(ct);
        }
    }
}
