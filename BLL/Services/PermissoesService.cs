using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Perfil;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class PermissoesService : IPermissoesService
    {
        private readonly IPermissoesRepository _repo;

        public PermissoesService(IPermissoesRepository repo) => _repo = repo;

        public async Task<List<PermissaoDto>> GetCatalogAsync(string? search, CancellationToken ct)
        {
            var query = _repo.Query().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                // ajuste os campos conforme existirem na sua tabela
                query = query.Where(x =>
                    x.Descricao.Contains(s));
            }

            return await query
                .OrderBy(x => x.IdPermissao)
                .Select(x => new PermissaoDto
                {
                    IdPermissao = x.IdPermissao,
                    Codigo = x.Codigo,
                    Ativo = x.Ativo,
                    Descricao = x.Descricao
                })
                .ToListAsync(ct);
        }
    }
}
