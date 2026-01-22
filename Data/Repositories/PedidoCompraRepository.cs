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

        public async Task<PagedResult<PedidoCompraListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _db.PedidosCompras.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(p => p.Observacao.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(p => p.DataEmissao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PedidoCompraListItemDto
                {
                    IdPedidoCompra = p.IdPedidoCompra,
                    DataPedido = p.DataEmissao.ToDateTime(new TimeOnly(0)),
                    FornecedorNome = _db.Fornecedores.Where(f => f.IdFornecedor == p.IdFornecedor).Select(f => f.Fantasia).FirstOrDefault() ?? string.Empty,
                    CondicaoPagamentoDescricao = _db.Condicoespagamentos.Where(c => c.IdCondicoespagamento == p.IdCondPagamento).Select(c => c.Descricao).FirstOrDefault() ?? string.Empty,
                    TotalPedido = p.TotalPedido
                })
                .ToListAsync(ct);

            return new PagedResult<PedidoCompraListItemDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<PedidoCompraDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _db.PedidosCompras
                .AsNoTracking()
                .Where(p => p.IdPedidoCompra == id)
                .Select(p => new PedidoCompraDto
                {
                    IdPedidoCompra = p.IdPedidoCompra,
                    DataPedido = p.DataEmissao.ToDateTime(new TimeOnly(0)),
                    IdFornecedor = p.IdFornecedor,
                    FornecedorNome = _db.Fornecedores.Where(f => f.IdFornecedor == p.IdFornecedor).Select(f => f.Fantasia).FirstOrDefault(),
                    IdCondicaoPagamento = p.IdCondPagamento ?? 0,
                    CondicaoPagamentoDescricao = _db.Condicoespagamentos.Where(c => c.IdCondicoespagamento == p.IdCondPagamento).Select(c => c.Descricao).FirstOrDefault(),
                    IdTransportadora = p.IdTransportadora,
                    TransportadoraNome = _db.Transportadoras.Where(t => t.IdTransportadora == p.IdTransportadora).Select(t => t.Fantasia).FirstOrDefault(),
                    TipoFrete = p.TipoFrete,
                    ValorFrete = p.ValorFrete,
                    TotalProdutos = p.TotalProdutos,
                    TotalIpi = p.TotalIpi,
                    TotalIcms = p.TotalIcms,
                    TotalPedido = p.TotalPedido,
                    Observacao = p.Observacao,
                    ObservacaoComplementar = p.ObservacaoCompl,
                    DataInclusao = p.DataCadastro,
                    DataAtualizacao = p.DataAlteracao,

                    Itens = _db.PedidosCompraItens.Where(i => i.IdPedidoCompra == p.IdPedidoCompra)
                        .Select(i => new PedidoCompraItemDto
                        {
                            IdPedidoCompraItem = i.IdPedidoCompraItem,
                            IdPedidoCompra = i.IdPedidoCompra,
                            IdProduto = i.IdProduto,
                            ProdutoCodigo = _db.Produtos.Where(pr => pr.IdProduto == i.IdProduto).Select(pr => pr.Codigo).FirstOrDefault(),
                            ProdutoDescricao = _db.Produtos.Where(pr => pr.IdProduto == i.IdProduto).Select(pr => pr.Descricao).FirstOrDefault(),
                            Quantidade = i.Quantidade,
                            PrecoUnitario = i.PrecoUnitario,
                            AliquotaIpi = i.AliquotaIpi,
                            AliquotaIcms = i.AliquotaIcms,
                            TotalItem = i.TotalItem,
                            TotalIpi = i.ValorIpi,
                            TotalIcms = i.ValorIcms
                        }).ToList()
                })
                .FirstOrDefaultAsync(ct);
        }

        public Task<PedidosCompra?> GetEntityByIdAsync(int id, CancellationToken ct)
            => _db.PedidosCompras.FirstOrDefaultAsync(p => p.IdPedidoCompra == id, ct);

        public async Task<int> AddAsync(PedidosCompra entity, List<PedidosCompraIten> itens, CancellationToken ct)
        {
            // set emission date if not provided
            if (entity.DataEmissao.Equals(default(DateOnly)) && entity.DataCadastro != default)
            {
                entity.DataEmissao = DateOnly.FromDateTime(entity.DataCadastro);
            }

            await _db.PedidosCompras.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);

            if (itens != null && itens.Count > 0)
            {
                foreach (var it in itens)
                {
                    it.IdPedidoCompra = entity.IdPedidoCompra;
                }
                await _db.PedidosCompraItens.AddRangeAsync(itens, ct);
                await _db.SaveChangesAsync(ct);
            }

            return entity.IdPedidoCompra;
        }

        public async Task UpdateAsync(PedidosCompra entity, List<PedidosCompraIten> itens, CancellationToken ct)
        {
            // remove existing itens
            _db.PedidosCompraItens.RemoveRange(_db.PedidosCompraItens.Where(i => i.IdPedidoCompra == entity.IdPedidoCompra));
            await _db.SaveChangesAsync(ct);

            if (itens != null && itens.Count > 0)
            {
                foreach (var it in itens)
                {
                    it.IdPedidoCompra = entity.IdPedidoCompra;
                }
                await _db.PedidosCompraItens.AddRangeAsync(itens, ct);
                await _db.SaveChangesAsync(ct);
            }

            _db.PedidosCompras.Update(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var p = await _db.PedidosCompras.FirstOrDefaultAsync(x => x.IdPedidoCompra == id, ct);
            if (p != null)
            {
                var itens = _db.PedidosCompraItens.Where(i => i.IdPedidoCompra == id);
                _db.PedidosCompraItens.RemoveRange(itens);
                _db.PedidosCompras.Remove(p);
                await _db.SaveChangesAsync(ct);
            }
        }
    }
}
