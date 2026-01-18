using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Perfil;
using Microsoft.EntityFrameworkCore;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class PerfisService : IPerfisService
    {
        private readonly IPerfisRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public PerfisService(IPerfisRepository repo, IMapper mapper, ICurrentUserService currentUser)
        {
            _repo = repo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<PerfilListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _repo.Query().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(x => x.Nome.Contains(s) || (x.Descricao ?? "").Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(x => x.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PerfilListItemDto
                {
                    IdPerfil = x.IdPerfil,
                    Nome = x.Nome,
                    Descricao = x.Descricao
                })
                .ToListAsync(ct);

            return new PagedResult<PerfilListItemDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public Task<List<PerfilLookupDto>> GetLookupAsync(string? search, CancellationToken ct)
        {
            var query = _repo.Query().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(x => x.Nome.Contains(s));
            }

            return query
                .OrderBy(x => x.Nome)
                .Take(50)
                .Select(x => new PerfilLookupDto { IdPerfil = x.IdPerfil, Nome = x.Nome })
                .ToListAsync(ct);
        }

        public async Task<int> CreateAsync(PerfilCreateDto dto, CancellationToken ct)
        {
            var existente = await _repo.GetByNomeAsync(dto.Nome, ct);
            if (existente != null)
                throw new InvalidOperationException("Já existe um perfil com este nome.");

            var entity = _mapper.Map<Models.Perfi>(dto);

            entity.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(entity, ct);
            await _repo.SaveAsync(ct);

            return entity.IdPerfil;
        }

        public async Task UpdateAsync(int id, PerfilUpdateDto dto, CancellationToken ct)
        {
            var entity = await _repo.GetAsync(id, ct);
            if (entity is null) throw new KeyNotFoundException("Perfil não encontrado.");

            var existente = await _repo.GetByNomeAsync(dto.Nome, ct);
            if (existente != null && existente.IdPerfil != id)
                throw new InvalidOperationException("Já existe um perfil com este nome.");

            _mapper.Map(dto, entity);
            
            await _repo.SaveAsync(ct);
        }

        public Task<List<int>> GetPermissoesAsync(int idPerfil, CancellationToken ct)
    => _repo.GetPermissoesAsync(idPerfil, ct);

        public async Task UpdatePermissoesAsync(int idPerfil, List<int> permissoesIds, CancellationToken ct)
        {
            var perfil = await _repo.GetAsync(idPerfil, ct);
            if (perfil is null) throw new KeyNotFoundException("Perfil não encontrado.");

            var ids = (permissoesIds ?? new List<int>())
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            var ok = await _repo.PermissoesExistemAsync(ids, ct);
            if (!ok) throw new InvalidOperationException("Uma ou mais permissões informadas não existem.");

            await _repo.ReplacePermissoesAsync(idPerfil, ids, ct);
            await _repo.SaveAsync(ct);
        }

    }
}
