using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class PrateleirasService : IPrateleirasService
    {
        private readonly IPrateleirasRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser; 

        public PrateleirasService(IPrateleirasRepository repo, IMapper mapper, ICurrentUserService currentUser)
        {
            _repo = repo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<Prateleira>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListPagedAsync(page, pageSize, search, ct);
        }

        public async Task<List<Prateleira>> GetListAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListAsync(search, ct);
        }

        public async Task<Prateleira> GetByIdAsync(int id, CancellationToken ct)
        {
            var p = await _repo.GetByIdAsync(id, ct);
            if (p is null) throw new KeyNotFoundException("Prateleira não encontrada.");

            return p;
        }

        public async Task<int?> CreateAsync(Prateleira dto, CancellationToken ct)
        {
            dto.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(dto, ct);
            await _repo.SaveAsync(ct);

            return dto.IdPrateleira;
        }

        public async Task UpdateAsync(int id, Prateleira dto, CancellationToken ct)
        {
            var p = await _repo.GetByIdAsync(id, ct);
            if (p is null) throw new KeyNotFoundException("Prateleira não encontrada.");

            p.Descricao = dto.Descricao;
            p.DescricaoNorm = dto.DescricaoNorm;

            p.EnsureUpdateAudit(_currentUser);
            await _repo.SaveAsync(ct);
        }
    }
}
