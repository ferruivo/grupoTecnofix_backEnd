using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.PedidoVenda;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Dtos;
using Microsoft.EntityFrameworkCore;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class PedidoVendaRepository : IPedidoVendaRepository
    {
        private readonly AppDbContext _db;

        public PedidoVendaRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<PedidoVendaListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _db.PedidosVenda.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(p => p.Observacoes != null && p.Observacoes.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(p => p.IdPedidoVenda)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PedidoVendaListItemDto
                {
                    IdPedidoVenda = p.IdPedidoVenda,
                    DataPedido = p.DataPedido,
                    NomeCliente = _db.Clientes.Where(c => c.IdCliente == p.IdCliente).Select(c => c.Fantasia).FirstOrDefault() ?? string.Empty,
                    NomeVendedor = (
                        from v in _db.Vendedores
                        join u in _db.Usuarios on v.IdUsuario equals u.IdUsuario
                        where v.IdVendedor == p.IdVendedor
                        select u.NomeCompleto
                    ).FirstOrDefault() ?? string.Empty,
                    TotalPedido = p.TotalPedido
                })
                .ToListAsync(ct);

            return new PagedResult<PedidoVendaListItemDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<PedidoVendaDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _db.PedidosVenda
                .AsNoTracking()
                .Where(p => p.IdPedidoVenda == id)
                .Select(p => new PedidoVendaDto
                {
                    IdPedidoVenda = p.IdPedidoVenda,
                    DataPedido = p.DataPedido,
                    IdCliente = p.IdCliente,
                    Cliente = _db.Clientes
                        .Where(c => c.IdCliente == p.IdCliente)
                        .Select(c => new ClienteDto
                        {
                            IdCliente = c.IdCliente,
                            Nome = c.Nome,
                            Fantasia = c.Fantasia,
                            Contato = c.Contato,
                            Email = c.Email,
                            Cep = c.Cep,
                            Endereco = c.Endereco,
                            Numero = c.Numero,
                            Complemento = c.Complemento,
                            Bairro = c.Bairro,
                            Municipio = _db.Municipios
                                .Where(m => m.IdMunicipio == c.IdMunicipio)
                                .Select(m => new MunicipioDto
                                {
                                    IdMunicipio = m.IdMunicipio,
                                    Nome = m.Nome,
                                    UF = m.Uf
                                })
                                .FirstOrDefault(),
                        })
                        .FirstOrDefault(),
                    IdVendedor = p.IdVendedor,
                    Vendedor = _db.Vendedores
                        .Where(v => v.IdVendedor == p.IdVendedor)
                        .Select(v => new VendedorDto
                        {
                            IdVendedor = v.IdVendedor,
                            IdUsuario = v.IdUsuario,
                            Interno = v.Interno,
                            Externo = v.Externo,
                            Observacao = v.Observacao ?? "",
                            Usuario = _db.Usuarios.Where(u => u.IdUsuario == v.IdUsuario)
                                .Select(u => new GrupoTecnofix_Api.Dtos.Usuario.UsuarioDto
                                {
                                    IdUsuario = u.IdUsuario,
                                    NomeCompleto = u.NomeCompleto,
                                    NomeExibicao = u.NomeExibicao,
                                    Login = u.Login,
                                    Email = u.Email,
                                    Ativo = u.Ativo
                                }).FirstOrDefault()!
                        })
                        .FirstOrDefault(),
                    IdCondicaoPagamento = p.IdCondicaoPagamento,
                    CondicaoPagamentoDescricao = _db.Condicoespagamentos.Where(c => c.IdCondicoespagamento == p.IdCondicaoPagamento).Select(c => c.Descricao).FirstOrDefault(),
                    CondicaoPagamento = _db.Condicoespagamentos
                        .Where(c => c.IdCondicoespagamento == p.IdCondicaoPagamento)
                        .Select(c => new CondicaoPagamentoDto
                        {
                            IdCondicoespagamento = c.IdCondicoespagamento,
                            Descricao = c.Descricao,
                        }).FirstOrDefault(),
                    IdTransportadora = p.IdTransportadora,
                    TransportadoraNome = _db.Transportadoras.Where(t => t.IdTransportadora == p.IdTransportadora).Select(t => t.Fantasia).FirstOrDefault(),
                    TipoFrete = p.TipoFrete,
                    ValorFrete = p.ValorFrete,
                    TotalProdutos = p.TotalProdutos,
                    TotalIpi = p.TotalIpi,
                    TotalIcms = p.TotalIcms,
                    TotalPedido = p.TotalPedido,
                    Observacoes = p.Observacoes,
                    DataInclusao = p.DataCadastro,
                    DataAtualizacao = p.DataAlteracao,

                    Itens = _db.PedidosVendaItens.Where(i => i.IdPedidoVenda == p.IdPedidoVenda)
                        .Select(i => new PedidoVendaItemDto
                        {
                            IdPedidoVendaItem = i.IdPedidoVendaItem,
                            IdProduto = i.IdProduto,
                            Produto = _db.Produtos.Where(pr => pr.IdProduto == i.IdProduto).Select(pr => new GrupoTecnofix_Api.Dtos.Produto.ProdutoDto
                            {
                                IdProduto = pr.IdProduto,
                                Codigo = pr.Codigo,
                                Descricao = pr.Descricao,
                                Ncm = pr.Ncm,
                                Unidade = pr.Unidade,
                                CstIpi = pr.CstIpi,
                                CstIcms = pr.CstIcms,
                                Minimo = pr.Minimo,
                                Obs = pr.Obs,
                                ObsNf = pr.ObsNf,
                                ObsEntrada = pr.ObsEntrada,
                                IS_KIT = pr.IS_KIT
                            }).FirstOrDefault(),
                            ProdutoCliente = i.ProdutoCliente,
                            Quantidade = i.Quantidade,
                            PrecoUnitario = i.PrecoUnitario,
                            NossoPrazo = i.NossoPrazo,
                            PrazoCliente = i.PrazoCliente,
                            PesoItem = i.PesoItem,
                            ValorKg = i.ValorKg,
                            AliquotaIpi = i.AliquotaIpi,
                            AliquotaIcms = i.AliquotaIcms,
                            TotalItem = i.TotalItem,
                            TotalIpi = i.TotalIpi,
                            TotalIcms = i.TotalIcms,
                            DataEntrega = i.DataEntrega
                        }).ToList()
                })
                .FirstOrDefaultAsync(ct);
        }

        public Task<PedidosVendum?> GetEntityByIdAsync(int id, CancellationToken ct)
            => _db.PedidosVenda.FirstOrDefaultAsync(p => p.IdPedidoVenda == id, ct);

        public async Task<int> AddAsync(PedidosVendum entity, List<PedidosVendaIten> itens, CancellationToken ct)
        {
            await _db.PedidosVenda.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);

            if (itens != null && itens.Count > 0)
            {
                foreach (var it in itens)
                {
                    it.IdPedidoVenda = entity.IdPedidoVenda;
                }
                await _db.PedidosVendaItens.AddRangeAsync(itens, ct);
                await _db.SaveChangesAsync(ct);
            }

            return entity.IdPedidoVenda;
        }

        public async Task UpdateAsync(PedidosVendum entity, List<PedidosVendaIten> itens, CancellationToken ct)
        {
            _db.PedidosVendaItens.RemoveRange(_db.PedidosVendaItens.Where(i => i.IdPedidoVenda == entity.IdPedidoVenda));
            await _db.SaveChangesAsync(ct);

            if (itens != null && itens.Count > 0)
            {
                foreach (var it in itens)
                {
                    it.IdPedidoVenda = entity.IdPedidoVenda;
                }
                await _db.PedidosVendaItens.AddRangeAsync(itens, ct);
                await _db.SaveChangesAsync(ct);
            }

            _db.PedidosVenda.Update(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync(ct);

            await _db.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM PEDIDOS_VENDA_ITENS WHERE ID_PEDIDO_VENDA = {id}", ct);
            await _db.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM PEDIDOS_VENDA WHERE ID_PEDIDO_VENDA = {id}", ct);

            await transaction.CommitAsync(ct);
        }
    }
}