using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Produto;
using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class ProdutosRepository : IProdutosRepository
    {
        private readonly AppDbContext _db;

        public ProdutosRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<ProdutoListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _db.Produtos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(c =>
                    c.Descricao.Contains(s) ||
                    c.Codigo.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(p => p.Descricao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProdutoListDto
                {
                    IdProduto = p.IdProduto,
                    Descricao = p.Descricao,
                    Codigo = p.Codigo
                })
                .ToListAsync(ct);

            return new PagedResult<ProdutoListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<List<ProdutoListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            var query = _db.Produtos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(f =>
                    (f.Descricao ?? "").Contains(s) ||
                    (f.Codigo ?? "").Contains(s));
            }

            var items = await query
                .OrderBy(f => f.Descricao)
                .Select(p => new ProdutoListDto
                {
                    IdProduto = p.IdProduto,
                    Descricao = p.Descricao,
                    Codigo = p.Codigo
                })
                .ToListAsync(ct);

            return items;
        }

        public Task<Produto?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Produtos.FirstOrDefaultAsync(x => x.IdProduto == id, ct);

        public Task AddAsync(Produto entity, CancellationToken ct)
        {
            return _db.Produtos.AddAsync(entity).AsTask();
        }

        public Task SaveAsync(CancellationToken ct)
        {
            try
            {
                return _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #region Preço venda
        public async Task<List<PrecoVendaDto>> GetListPrecoVendaAsync(int idCliente, CancellationToken ct)
        {
            var items = await _db.Precovenda
                .AsNoTracking()
                .Where(p => p.IdCliente == idCliente)
                .Select(p => new PrecoVendaDto
                {
                    IdPrecovenda = p.IdPrecovenda,
                    IdProduto = p.IdProduto,
                    IdCliente = p.IdCliente,
                    Preco = p.Preco,
                    Vigencia = p.Vigencia,
                    Revisao = p.Revisao,
                    Datarevisao = p.Datarevisao,
                    Usurevisao = p.Usurevisao,
                    Precoantigo = p.Precoantigo,
                    Obs = p.Obs,
                    DataCadastro = p.DataCadastro,
                    DataAlteracao = p.DataAlteracao,


                    // =========================
                    // Subquery: Produto
                    // =========================
                    Produto = _db.Produtos
                        .Where(pv => pv.IdProduto == p.IdProduto)
                        .Select(pv => new ProdutoListDto
                        {
                            IdProduto = pv.IdProduto,
                            Codigo = pv.Codigo,
                            Descricao = pv.Descricao,
                        })
                        .FirstOrDefault(),

                })
                .ToListAsync(ct);

            return items;
        }

        public Task<Precovendum?> GetPrecoVendaByIdAsync(int id, CancellationToken ct)
            => _db.Precovenda.FirstOrDefaultAsync(x => x.IdPrecovenda == id, ct);

        public Task AddAsync(Precovendum entity, CancellationToken ct)
        {
            return _db.Precovenda.AddAsync(entity).AsTask();
        }

        #endregion

        #region Preço compra
        public async Task<List<PrecoCompraDto>> GetListPrecoCompraAsync(int idFornecedor, CancellationToken ct)
        {
            var items = await _db.Precocompras
                .AsNoTracking()
                .Where(p => p.IdFornecedor == idFornecedor)
                .Select(p => new PrecoCompraDto
                {
                    IdPrecocompra = p.IdPrecocompra,
                    IdProduto = p.IdProduto,
                    IdFornecedor = p.IdFornecedor,
                    Preco = p.Preco,
                    Vigencia = p.Vigencia,
                    Precoantigo = p.Precoantigo,
                    Obs = p.Obs,
                    DataCadastro = p.DataCadastro,
                    DataAlteracao = p.DataAlteracao,

                    // =========================
                    // Subquery: Produto
                    // =========================
                    Produto = _db.Produtos
                        .Where(pv => pv.IdProduto == p.IdProduto)
                        .Select(pv => new ProdutoListDto
                        {
                            IdProduto = pv.IdProduto,
                            Codigo = pv.Codigo,
                            Descricao = pv.Descricao,
                        })
                        .FirstOrDefault(),

                })
                .ToListAsync(ct);

            return items;
        }

        public Task<Precocompra?> GetPrecoCompraByIdAsync(int id, CancellationToken ct)
            => _db.Precocompras.FirstOrDefaultAsync(x => x.IdPrecocompra == id, ct);

        public Task AddAsync(Precocompra entity, CancellationToken ct)
        {
            return _db.Precocompras.AddAsync(entity).AsTask();
        }

        #endregion


    }
}
