using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.PedidoCompra;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class PedidoCompraService : IPedidoCompraService
    {
        private readonly IPedidoCompraRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public PedidoCompraService(IPedidoCompraRepository repo, ICurrentUserService currentUser, IMapper mapper)
        {
            _repo = repo;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<PagedResult<PedidoCompraListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetPagedAsync(page, pageSize, search, ct);
        }

        public async Task<PedidoCompraDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _repo.GetByIdAsync(id, ct);
        }

        public async Task<int> CreateAsync(PedidoCompraCreateUpdateDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<PedidosCompra>(dto);
            entity.EnsureCreationAudit(_currentUser);
            
            // map items
            var itens = dto.Itens.Select(i => _mapper.Map<PedidosCompraIten>(i)).ToList();
            foreach (var it in itens)
            {
                it.EnsureCreationAudit(_currentUser);
            }

            entity.EnsureCreationAudit(_currentUser);

            return await _repo.AddAsync(entity, itens, ct);
        }

        public async Task UpdateAsync(int id, PedidoCompraCreateUpdateDto dto, CancellationToken ct)
        {
            var existingEntity = await _repo.GetEntityByIdAsync(id, ct);
            if (existingEntity is null) throw new KeyNotFoundException("Pedido de compra não encontrado.");

            // map main fields onto existing entity
            _mapper.Map(dto, existingEntity);
            existingEntity.DataEmissao = DateOnly.FromDateTime(dto.DataPedido == default ? DateTime.Now : dto.DataPedido);
            existingEntity.IdUsuarioAlteracao = _currentUser.GetUsuarioLogadoId();
            existingEntity.DataAlteracao = DateTime.Now;

            var itens = dto.Itens.Select(i => _mapper.Map<PedidosCompraIten>(i)).ToList();
            foreach (var it in itens)
            {
                it.IdUsuarioCadastro = _currentUser.GetUsuarioLogadoId();
                it.DataCadastro = DateTime.Now;
            }

            existingEntity.EnsureUpdateAudit(_currentUser);

            await _repo.UpdateAsync(existingEntity, itens, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            await _repo.DeleteAsync(id, ct);
        }
    }
}
