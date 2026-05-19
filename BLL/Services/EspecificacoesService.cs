using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class EspecificacoesService : IEspecificacoesService
    {
        private readonly IEspecificacoesRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public EspecificacoesService(IEspecificacoesRepository repo, IMapper mapper, ICurrentUserService currentUser)
        {
            _repo = repo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<EspecificacaoListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListPagedAsync(page, pageSize, search, ct);
        }

        public async Task<List<EspecificacaoListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListAsync(search, ct);
        }

        public async Task<EspecificacaoDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var e = await _repo.GetByIdAsync(id, ct);
            if (e is null) throw new KeyNotFoundException("Especificação não encontrada.");

            return _mapper.Map<EspecificacaoDto>(e);
        }

        public async Task<int> CreateAsync(EspecificacaoDto dto, CancellationToken ct)
        {
            var e = _mapper.Map<Especificacao>(dto);
            e.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(e, ct);
            await _repo.SaveAsync(ct);

            return e.IdEspecificacao;
        }

        public async Task UpdateAsync(int id, EspecificacaoDto dto, CancellationToken ct)
        {
            var e = await _repo.GetByIdAsync(id, ct);
            if (e is null) throw new KeyNotFoundException("Especificação não encontrada.");

            _mapper.Map(dto, e);

            await _repo.SaveAsync(ct);
        }
    }
}
