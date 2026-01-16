using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Condição_Pagamento;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class CondicoesPagamentoService : ICondicoesPagamentoService
    {
        private readonly ICondicoesPagamentoRepository _repo;
        private readonly IMapper _mapper;

        public CondicoesPagamentoService(ICondicoesPagamentoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<CondicaoPagamentoListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListPagedAsync(page, pageSize, search, ct);
        }

        public async Task<List<CondicaoPagamentoListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListAsync(search, ct);
        }

        public async Task<CondicaoPagamentoDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var u = await _repo.GetByIdAsync(id, ct);
            if (u is null) throw new KeyNotFoundException("Condição de pagamento não encontrada.");

            var dto = _mapper.Map<CondicaoPagamentoDto>(u);

            return dto;
        }

        public async Task<int> CreateAsync(CondicaoPagamentoDto dto, CancellationToken ct)
        {
            var cp = _mapper.Map<Condicoespagamento>(dto);
            
            await _repo.AddAsync(cp, ct);
            await _repo.SaveAsync(ct);

            return cp.IdCondicoespagamento;
        }

        public async Task UpdateAsync(int id, CondicaoPagamentoDto dto, CancellationToken ct)
        {
            var c = await _repo.GetByIdAsync(id, ct);
            if (c is null) throw new KeyNotFoundException("Condição de pagamento não encontrada.");

            _mapper.Map(dto, c);

            await _repo.SaveAsync(ct);
        }
    }
}
