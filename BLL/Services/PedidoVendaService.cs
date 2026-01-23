using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.PedidoVenda;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class PedidoVendaService : IPedidoVendaService
    {
        private readonly IPedidoVendaRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public PedidoVendaService(IPedidoVendaRepository repo, IMapper mapper, ICurrentUserService currentUser)
        {
            _repo = repo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<PedidoVendaListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetPagedAsync(page, pageSize, search, ct);
        }

        public async Task<PedidoVendaDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _repo.GetByIdAsync(id, ct);
        }

        public async Task<int> CreateAsync(PedidoVendaCreateUpdateDto dto, CancellationToken ct)
        {
            // map main entity using AutoMapper
            var entity = _mapper.Map<PedidosVendum>(dto);

            // ensure creation audit on entity
            entity.EnsureCreationAudit(_currentUser);

            // map items and ensure creation audit on each
            var itens = dto.Itens?.Select(i => _mapper.Map<PedidosVendaIten>(i)).ToList() ?? new List<PedidosVendaIten>();
            foreach (var it in itens)
            {
                it.EnsureCreationAudit(_currentUser);
            }

            var id = await _repo.AddAsync(entity, itens, ct);
            return id;
        }

        public async Task UpdateAsync(int id, PedidoVendaCreateUpdateDto dto, CancellationToken ct)
        {
            var entity = await _repo.GetEntityByIdAsync(id, ct);
            if (entity is null) throw new KeyNotFoundException("Pedido venda não encontrado.");

            // map DTO onto existing entity
            _mapper.Map(dto, entity);

            // ensure DataPedido fallback
            if (dto.DataPedido == default) entity.DataPedido = DateTime.Now;

            // set update audit on main entity
            entity.EnsureUpdateAudit(_currentUser);

            // map items and ensure creation audit for each (they will be re-inserted)
            var itens = dto.Itens?.Select(i => _mapper.Map<PedidosVendaIten>(i)).ToList() ?? new List<PedidosVendaIten>();
            foreach (var it in itens)
            {
                it.EnsureCreationAudit(_currentUser);
            }

            await _repo.UpdateAsync(entity, itens, ct);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var e = await _repo.GetEntityByIdAsync(id, ct);
            if (e is null) return false;

            await _repo.DeleteAsync(id, ct);
            return true;
        }
    }
}