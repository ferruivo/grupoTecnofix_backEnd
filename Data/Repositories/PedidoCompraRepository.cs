using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.PedidoCompra;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class PedidoCompraRepository : IPedidoCompraRepository
    {
        private readonly AppDbContext _db;

        public PedidoCompraRepository(AppDbContext db) => _db = db;

        //public async Task<PagedResult<PedidoCompraListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        //{
        //    var query = _db.PedidoCompras.AsNoTracking();

        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        var s = search.Trim();
        //        query = query.Where(p => p.Observacao.Contains(s));
        //    }

        //    var total = await query.CountAsync(ct);

        //    var items = await query
        //        .OrderByDescending(p => p.DataPedido)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .Select(p => new PedidoCompraListItemDto
        //        {
        //            IdPedidoCompra = p.IdPedidoCompra,
        //            DataPedido = p.DataPedido,
        //            FornecedorNome = _db.Fornecedores.Where(f => f.IdFornecedor == p.IdFornecedor).Select(f => f.Fantasia).FirstOrDefault() ?? string.Empty,
        //            CondicaoPagamentoDescricao = _db.Condicoespagamentos.Where(c => c.IdCondicoespagamento == p.IdCondicaoPagamento).Select(c => c.Descricao).FirstOrDefault() ?? string.Empty,
        //            TotalPedido = p.TotalPedido
        //        })
        //        .ToListAsync(ct);

        //    return new PagedResult<PedidoCompraListItemDto>
        //    {
        //        Page = page,
        //        PageSize = pageSize,
        //        TotalItems = total,
        //        Items = items
        //    };
        //}

        //public async Task<PedidoCompraDto?> GetByIdAsync(int id, CancellationToken ct)
        //{
        //    return await _db.PedidoCompras
        //        .AsNoTracking()
        //        .Where(p => p.IdPedidoCompra == id)
        //        .Select(p => new PedidoCompraDto
        //        {
        //            IdPedidoCompra = p.IdPedidoCompra,
        //            DataPedido = p.DataPedido,
        //            IdFornecedor = p.IdFornecedor,
        //            FornecedorNome = _db.Fornecedores.Where(f => f.IdFornecedor == p.IdFornecedor).Select(f => f.Fantasia).FirstOrDefault(),
        //            IdCondicaoPagamento = p.IdCondicaoPagamento,
        //            CondicaoPagamentoDescricao = _db.Condicoespagamentos.Where(c => c.IdCondicoespagamento == p.IdCondicaoPagamento).Select(c => c.Descricao).FirstOrDefault(),
        //            IdTransportadora = p.IdTransportadora,
        //            TransportadoraNome = _db.Transportadoras.Where(t => t.IdTransportadora == p.IdTransportadora).Select(t => t.Fantasia).FirstOrDefault(),
        //            TipoFrete = p.TipoFrete,
        //            ValorFrete = p.ValorFrete,
        //            TotalProdutos = p.TotalProdutos,
        //            TotalIpi = p.TotalIpi,
        //            TotalIcms = p.TotalIcms,
        //            TotalPedido = p.TotalPedido,
        //            Observacao = p.Observacao,
        //            ObservacaoComplementar = p.ObservacaoComplementar,
        //            DataInclusao = p.DataInclusao,
        //            DataAtualizacao = p.DataAtualizacao,

        //            Itens = _db.PedidoCompraItens.Where(i => i.IdPedidoCompra == p.IdPedidoCompra)
        //                .Select(i => new PedidoCompraItemDto
        //                {
        //                    IdPedidoCompraItem = i.IdPedidoCompraItem,
        //                    IdPedidoCompra = i.IdPedidoCompra,
        //                    IdProduto = i.IdProduto,
        //                    ProdutoCodigo = _db.Produtos.Where(pr => pr.IdProduto == i.IdProduto).Select(pr => pr.Codigo).FirstOrDefault(),
        //                    ProdutoDescricao = _db.Produtos.Where(pr => pr.IdProduto == i.IdProduto).Select(pr => pr.Descricao).FirstOrDefault(),
        //                    Quantidade = i.Quantidade,
        //                    PrecoUnitario = i.PrecoUnitario,
        //                    AliquotaIpi = i.AliquotaIpi,
        //                    AliquotaIcms = i.AliquotaIcms,
        //                    TotalItem = i.TotalItem,
        //                    TotalIpi = i.TotalIpi,
        //                    TotalIcms = i.TotalIcms
        //                }).ToList()
        //        })
        //        .FirstOrDefaultAsync(ct);
        //}

        //public Task<PedidoCompra?> GetEntityByIdAsync(int id, CancellationToken ct)
        //    => _db.PedidoCompras.Include(p => p.Itens).FirstOrDefaultAsync(p => p.IdPedidoCompra == id, ct);

        //public async Task<int> AddAsync(PedidoCompra entity, CancellationToken ct)
        //{
        //    await _db.PedidoCompras.AddAsync(entity, ct);
        //    await _db.SaveChangesAsync(ct);
        //    return entity.IdPedidoCompra;
        //}

        //public async Task UpdateAsync(PedidoCompra entity, CancellationToken ct)
        //{
        //    _db.PedidoCompraItens.RemoveRange(_db.PedidoCompraItens.Where(i => i.IdPedidoCompra == entity.IdPedidoCompra));
        //    await _db.SaveChangesAsync(ct);

        //    await _db.PedidoCompraItens.AddRangeAsync(entity.Itens, ct);

        //    _db.PedidoCompras.Update(entity);
        //    await _db.SaveChangesAsync(ct);
        //}

        //public async Task DeleteAsync(int id, CancellationToken ct)
        //{
        //    var p = await _db.PedidoCompras.Include(p => p.Itens).FirstOrDefaultAsync(x => x.IdPedidoCompra == id, ct);
        //    if (p != null)
        //    {
        //        _db.PedidoCompraItens.RemoveRange(p.Itens);
        //        _db.PedidoCompras.Remove(p);
        //        await _db.SaveChangesAsync(ct);
        //    }
        //}
    }
}
