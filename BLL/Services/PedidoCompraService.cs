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

    //    public async Task<PagedResult<PedidoCompraListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
    //    {
    //        if (page < 1) page = 1;
    //        if (pageSize < 1) pageSize = 20;
    //        if (pageSize > 200) pageSize = 200;

    //        return await _repo.GetPagedAsync(page, pageSize, search, ct);
    //    }

    //    public async Task<PedidoCompraDto?> GetByIdAsync(int id, CancellationToken ct)
    //    {
    //        return await _repo.GetByIdAsync(id, ct);
    //    }

    //    public async Task<int> CreateAsync(PedidoCompraCreateUpdateDto dto, CancellationToken ct)
    //    {
    //        var model = _mapper.Map<PedidoCompra>(dto);
    //        model.DataInclusao = DateTime.Now;
    //        model.Itens = dto.Itens.Select(i => new PedidoCompraItem
    //        {
    //            IdProduto = i.IdProduto,
    //            Quantidade = i.Quantidade,
    //            PrecoUnitario = i.PrecoUnitario,
    //            AliquotaIpi = i.AliquotaIpi,
    //            AliquotaIcms = i.AliquotaIcms,
    //            TotalItem = i.TotalItem,
    //            TotalIpi = i.TotalIpi,
    //            TotalIcms = i.TotalIcms
    //        }).ToList();

    //        return await _repo.AddAsync(model, ct);
    //    }

    //    public async Task UpdateAsync(int id, PedidoCompraCreateUpdateDto dto, CancellationToken ct)
    //    {
    //        var existing = await _repo.GetByIdAsync(id, ct);
    //        if (existing is null) throw new KeyNotFoundException("Pedido de compra não encontrado.");

    //        // map fields
    //        existing.DataPedido = dto.DataPedido;
    //        existing.IdFornecedor = dto.IdFornecedor;
    //        existing.IdCondicaoPagamento = dto.IdCondicaoPagamento;
    //        existing.IdTransportadora = dto.IdTransportadora;
    //        existing.TipoFrete = dto.TipoFrete;
    //        existing.ValorFrete = dto.ValorFrete;
    //        existing.TotalProdutos = dto.TotalProdutos;
    //        existing.TotalIpi = dto.TotalIpi;
    //        existing.TotalIcms = dto.TotalIcms;
    //        existing.TotalPedido = dto.TotalPedido;
    //        existing.Observacao = dto.Observacao;
    //        existing.ObservacaoComplementar = dto.ObservacaoComplementar;
    //        existing.DataAtualizacao = DateTime.Now;

    //        // replace items: for simplicity delete and re-add
    //        // remove existing items
    //        // (repository currently doesn't expose item operations; keep it simple)

    //        // map new items
    //        existing.Itens = dto.Itens.Select(i => new PedidoCompraItem
    //        {
    //            IdProduto = i.IdProduto,
    //            Quantidade = i.Quantidade,
    //            PrecoUnitario = i.PrecoUnitario,
    //            AliquotaIpi = i.AliquotaIpi,
    //            AliquotaIcms = i.AliquotaIcms,
    //            TotalItem = i.TotalItem,
    //            TotalIpi = i.TotalIpi,
    //            TotalIcms = i.TotalIcms
    //        }).ToList();

    //        await _repo.UpdateAsync(existing, ct);
    //    }

    //    public async Task DeleteAsync(int id, CancellationToken ct)
    //    {
    //        await _repo.DeleteAsync(id, ct);
    //    }
    }
}
