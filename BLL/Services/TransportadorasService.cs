using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class TransportadorasService : ITransportadorasService
    {
        private readonly ITransportadorasRepository _repo;
        private readonly IMapper _mapper;

        public TransportadorasService(ITransportadorasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<TransportadoraListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListAsync(page, pageSize, search, ct);
        }

        public async Task<int> CreateAsync(TransportadoraCreateDto dto, CancellationToken ct)
        {
            var transp = _mapper.Map<Transportadora>(dto);

            await _repo.AddAsync(transp, ct);
            await _repo.SaveAsync(ct);

            return transp.IdTransportadora;
        }

        public async Task UpdateAsync(int id, TransportadoraUpdateDto dto, CancellationToken ct)
        {
            var t = await _repo.GetByIdAsync(id, ct);
            if (t is null) throw new KeyNotFoundException("Transportadora não encontrada.");

            _mapper.Map(dto, t);

            await _repo.SaveAsync(ct);
        }
    }
}
