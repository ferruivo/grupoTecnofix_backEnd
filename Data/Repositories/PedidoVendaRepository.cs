using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.ParametroVenda;
using GrupoTecnofix_Api.Dtos.PedidoVenda;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class PedidoVendaRepository : IPedidoVendaRepository
    {
        private readonly AppDbContext _db;

        public PedidoVendaRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<PedidoVendaListItemDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var baseQuery =
                from p in _db.PedidosVenda.AsNoTracking()
                join c in _db.Clientes.AsNoTracking()
                    on p.IdCliente equals c.IdCliente into cj
                from c in cj.DefaultIfEmpty()
                join v in _db.Vendedores.AsNoTracking()
                    on p.IdVendedor equals v.IdVendedor into vj
                from v in vj.DefaultIfEmpty()
                join u in _db.Usuarios.AsNoTracking()
                    on v.IdUsuario equals u.IdUsuario into uj
                from u in uj.DefaultIfEmpty()
                select new
                {
                    p,
                    ClienteNome = c != null ? c.Fantasia : null,
                    VendedorNome = u != null ? u.NomeCompleto : null
                };

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();

                baseQuery = baseQuery.Where(x =>
                    // número do pedido
                    x.p.IdPedidoVenda.ToString().Contains(s) ||
                    // cliente
                    (x.ClienteNome != null && x.ClienteNome.Contains(s)) ||
                    // vendedor
                    (x.VendedorNome != null && x.VendedorNome.Contains(s)) ||
                    // (opcional) observações
                    (x.p.Observacoes != null && x.p.Observacoes.Contains(s))
                );
            }

            var total = await baseQuery.CountAsync(ct);

            var items = await baseQuery
                .OrderByDescending(x => x.p.IdPedidoVenda)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PedidoVendaListItemDto
                {
                    IdPedidoVenda = x.p.IdPedidoVenda,
                    DataPedido = x.p.DataPedido,
                    NomeCliente = x.ClienteNome ?? string.Empty,
                    NomeVendedor = x.VendedorNome ?? string.Empty,
                    TotalPedido = x.p.TotalPedido
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

        public async Task<List<PedidoVendaDisponivelDto>> GetDisponiveisAsync(long? idProduto, long? idCliente,CancellationToken ct)
        {
            var query =
                from pv in _db.PedidosVenda.AsNoTracking()

                join pvi in _db.PedidosVendaItens.AsNoTracking()
                    on pv.IdPedidoVenda equals pvi.IdPedidoVenda

                join c in _db.Clientes.AsNoTracking()
                    on pv.IdCliente equals c.IdCliente into cj
                from c in cj.DefaultIfEmpty()

                where pvi.IdProduto == idProduto
                      && (idCliente == null || pv.IdCliente == idCliente)

                let entregue = _db.Consumolotes
                    .Where(cl =>
                        cl.Idpedidovenda == pvi.IdPedidoVenda &&
                        cl.Item == pvi.Item)
                    .Sum(cl => (decimal?)cl.Quantidade) ?? 0

                let diferenca = pvi.Quantidade - entregue

                where diferenca > 0

                orderby pv.IdPedidoVenda descending

                select new PedidoVendaDisponivelDto
                {
                    Data = pv.DataPedido.ToString("dd/MM/yyyy HH:mm"),
                    Cliente = c != null ? c.Fantasia : null,
                    Pedido = pvi.IdPedidoVenda,
                    Item = pvi.Item,
                    Quantidade = pvi.Quantidade,
                    Entregue = entregue,
                    Diferenca = diferenca
                };

            return await query.ToListAsync(ct);
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
                            IdPedidoVendaItem = i.Item,
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
                            DataEntrega = i.DataEntrega,
                            NumeroPedidoCliente = i.NumeroPedidoCliente,
                            ItemPedidoCliente = i.ItemPedidoCliente
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

        public async Task<PagedResult<PedidoVendaPendenteItemDto>> GetPendentesLiberacaoAsync(int idCliente, int page, int pageSize, CancellationToken ct)
        {
            var baseQuery =
                from i in _db.PedidosVendaItens.AsNoTracking()
                join p in _db.PedidosVenda.AsNoTracking()
                    on i.IdPedidoVenda equals p.IdPedidoVenda
                join pr in _db.Produtos.AsNoTracking()
                    on i.IdProduto equals pr.IdProduto
                where p.IdCliente == idCliente
                select new
                {
                    Pedido = p,
                    Item = i,
                    Produto = pr
                };

            var total = await baseQuery.CountAsync(ct);

            var items = await baseQuery
                .OrderBy(x => x.Pedido.IdPedidoVenda)
                .ThenBy(x => x.Item.Item)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PedidoVendaPendenteItemDto
                {
                    IdPedidoVenda = x.Pedido.IdPedidoVenda,
                    NumeroPedido = x.Pedido.IdPedidoVenda,

                    Item = x.Item.Item.ToString(),

                    IdProduto = x.Item.IdProduto,
                    ProdutoCodigo = x.Produto.Codigo,
                    ProdutoDescricao = x.Produto.Descricao,

                    Quantidade = x.Item.Quantidade,


                    Disponivel = x.Item.Quantidade - (
                        from c in _db.Consumolotes.AsNoTracking()
                        where c.Idpedidovenda == x.Item.IdPedidoVenda
                        && c.Item == x.Item.Item
                        select (decimal?)c.Quantidade
                    ).Sum() ?? 0,

                    Liberado = (
                        from c in _db.Consumolotes.AsNoTracking()
                        where c.Idpedidovenda == x.Item.IdPedidoVenda
                        && c.Item == x.Item.Item
                        select (decimal?)c.Quantidade
                    ).Sum() ?? 0,

                    ProdutoCliente = x.Item.ProdutoCliente,
                    NossoPrazo = x.Item.NossoPrazo,
                    PrazoCliente = x.Item.PrazoCliente
                })
                .ToListAsync(ct);

            return new PagedResult<PedidoVendaPendenteItemDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<PedidoVendaItemLiberacaoBaseDto?> GetItemPedidoVendaAsync(int idPedidoVenda,int item,CancellationToken ct)
        {
            return await _db.PedidosVendaItens
                .AsNoTracking()
                .Where(i =>
                    i.IdPedidoVenda == idPedidoVenda &&
                    ((i.Item) == item))
                .Select(i => new PedidoVendaItemLiberacaoBaseDto
                {
                    IdPedidoVenda = i.IdPedidoVenda,
                    Item = i.Item,
                    IdProduto = i.IdProduto,
                    Quantidade = i.Quantidade
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<LiberarPendenteResponseDto> LiberarPendenteAsync(LiberarPendenteRequestDto dto,int? idUsuario,CancellationToken ct)
        {
            var strategy = _db.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _db.Database.BeginTransactionAsync(ct);

                var itemPedido = await _db.PedidosVendaItens
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.IdPedidoVenda == dto.IdPedidoVenda &&
                        x.Item == dto.Item &&
                        x.IdProduto == dto.IdProduto,
                        ct);

                var totalLiberar = dto.Alocacoes.Sum(x => x.Quantidade);

               
                var response = new LiberarPendenteResponseDto
                {
                    IdPedidoVenda = dto.IdPedidoVenda,
                    Item = dto.Item,
                    Quantidade = itemPedido.Quantidade,
                    Liberado = totalLiberar,
                    Status = "L"
                };

                foreach (var alocacao in dto.Alocacoes)
                {
                    var lote = await _db.Lotes
                        .FirstOrDefaultAsync(x =>
                            x.IdLote == alocacao.IdLote &&
                            x.IdProduto == dto.IdProduto,
                            ct);

                    var qtdUtilizada = lote.QtdUtilizada ?? 0;
                    var disponivel = lote.Quantidade - qtdUtilizada;

                  

                    var consumo = new Consumolote
                    {
                        Idpedidovenda = dto.IdPedidoVenda,
                        Item = dto.Item,
                        Idproduto = dto.IdProduto,
                        Idlote = lote.IdLote,
                        Quantidade = alocacao.Quantidade,
                        Data = DateOnly.FromDateTime(DateTime.Now),
                        Usuario = idUsuario?.ToString() ?? "SISTEMA",
                        Notafiscal = -1
                    };

                    await _db.Consumolotes.AddAsync(consumo, ct);

                    lote.QtdUtilizada = qtdUtilizada + alocacao.Quantidade;
                    _db.Lotes.Update(lote);

                    response.Alocacoes.Add(new LiberarPendenteAlocacaoResponseDto
                    {
                        IdLote = lote.IdLote,
                        Quantidade = alocacao.Quantidade,
                        NumeroLote = lote.IdLote.ToString()
                    });
                }

                await _db.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

                return response;
            });
        }

        public async Task<List<LiberacaoAlocacoesDto>> GetAlocacoesLiberadasAsync(int idPedidoVenda,int? item,CancellationToken ct)
        {
            var consumos = await (
                from c in _db.Consumolotes.AsNoTracking()
                join l in _db.Lotes.AsNoTracking()
                    on c.Idlote equals l.IdLote
                where c.Idpedidovenda == idPedidoVenda && c.Notafiscal == -1
                orderby c.Item, c.IdConsumolote
                select new
                {
                    c.Item,
                    c.Idproduto,
                    c.Idlote,
                    c.Quantidade,
                    NumeroLote = l.IdLote.ToString()
                })
                .ToListAsync(ct);

            if (consumos.Count == 0)
                return new List<LiberacaoAlocacoesDto>();

            var itensConsumidos = consumos
                .Select(c => c.Item)
                .Distinct()
                .ToList();

            var itensPedido = await _db.PedidosVendaItens
                .AsNoTracking()
                .Where(i =>
                    i.IdPedidoVenda == idPedidoVenda &&
                    itensConsumidos.Contains(i.Item))
                .Select(i => new
                {
                    i.IdPedidoVenda,
                    Item = i.Item,
                    i.IdProduto,
                    i.Quantidade
                })
                .ToListAsync(ct);

            var result = itensPedido
                .Select(i =>
                {
                    var alocacoesItem = consumos
                        .Where(c => c.Item == i.Item)
                        .Select(c => new LiberacaoAlocacaoDto
                        {
                            IdLote = c.Idlote,
                            NumeroLote = c.NumeroLote,
                            Quantidade = c.Quantidade,
                            FornecedorNome = string.Empty
                        })
                        .ToList();

                    return new LiberacaoAlocacoesDto
                    {
                        IdPedidoVenda = i.IdPedidoVenda,
                        Item = i.Item,
                        IdProduto = i.IdProduto,
                        Quantidade = i.Quantidade,
                        Liberado = alocacoesItem.Sum(a => a.Quantidade),
                        Alocacoes = alocacoesItem
                    };
                })
                .Where(x => x.Alocacoes.Count > 0)
                .ToList();

            return result;
        }

        public async Task<EstornarLiberacaoResponseDto> EstornarLiberacaoAsync(EstornarLiberacaoRequestDto dto,CancellationToken ct)
        {
            var strategy = _db.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _db.Database.BeginTransactionAsync(ct);

                var consumos = await _db.Consumolotes
                    .Where(c =>
                        c.Idpedidovenda == dto.IdPedidoVenda &&
                        c.Item == dto.Item &&
                        c.Idlote == dto.IdLote)
                    .OrderByDescending(c => c.IdConsumolote)
                    .ToListAsync(ct);

                if (consumos.Count == 0)
                    throw new KeyNotFoundException("Liberação do lote não encontrada.");

                var totalConsumidoLote = consumos.Sum(c => c.Quantidade);
                var quantidadeEstornar = dto.Quantidade ?? totalConsumidoLote;

                if (quantidadeEstornar > totalConsumidoLote)
                    throw new InvalidOperationException("Quantidade de estorno maior que a quantidade liberada para o lote.");

                var lote = await _db.Lotes
                    .FirstOrDefaultAsync(l => l.IdLote == dto.IdLote, ct);

                if (lote is null)
                    throw new KeyNotFoundException("Lote não encontrado.");

                var restanteEstornar = quantidadeEstornar;

                foreach (var consumo in consumos)
                {
                    if (restanteEstornar <= 0)
                        break;

                    if (consumo.Quantidade <= restanteEstornar)
                    {
                        restanteEstornar -= consumo.Quantidade;
                        _db.Consumolotes.Remove(consumo);
                    }
                    else
                    {
                        consumo.Quantidade -= restanteEstornar;
                        restanteEstornar = 0;
                        _db.Consumolotes.Update(consumo);
                    }
                }

                lote.QtdUtilizada = (lote.QtdUtilizada ?? 0) - quantidadeEstornar;

                if (lote.QtdUtilizada < 0)
                    lote.QtdUtilizada = 0;

                _db.Lotes.Update(lote);

                await _db.SaveChangesAsync(ct);

                var liberadoAtual = await _db.Consumolotes
                    .AsNoTracking()
                    .Where(c =>
                        c.Idpedidovenda == dto.IdPedidoVenda &&
                        c.Item == dto.Item)
                    .SumAsync(c => (decimal?)c.Quantidade, ct) ?? 0;

                var status = liberadoAtual == 0 ? "P" : "X";

                await transaction.CommitAsync(ct);

                return new EstornarLiberacaoResponseDto
                {
                    IdPedidoVenda = dto.IdPedidoVenda,
                    Item = dto.Item,
                    Liberado = liberadoAtual,
                    Status = status
                };
            });
        }

        public async Task<ExpedicaoConsumoLoteDto?> GetExpedicaoConsumoLoteAsync(int idCliente,CancellationToken ct)
        {
            var dados = await (
                from c in _db.Consumolotes.AsNoTracking()
                join pvi in _db.PedidosVendaItens.AsNoTracking()
                    on new { IdPedidoVenda = c.Idpedidovenda, Item = c.Item }
                    equals new { IdPedidoVenda = pvi.IdPedidoVenda, Item = pvi.Item }
                join pv in _db.PedidosVenda.AsNoTracking()
                    on pvi.IdPedidoVenda equals pv.IdPedidoVenda
                join p in _db.Produtos.AsNoTracking()
                    on pvi.IdProduto equals p.IdProduto
                join cli in _db.Clientes.AsNoTracking()
                    on pv.IdCliente equals cli.IdCliente
                join m in _db.Municipios.AsNoTracking()
                    on cli.IdMunicipio equals m.IdMunicipio
                join t in _db.Transportadoras.AsNoTracking()
                on cli.IdTransportadora equals t.IdTransportadora
                where c.Notafiscal == -1
                      && pv.IdCliente == idCliente
                select new
                {
                    cli.Nome,
                    cli.Contato,
                    Municipio = m.Nome,
                    Uf = m.Uf,
                    cli.ObservacaoOrdemExpedicao,
                    TransportadoraNome = t.Fantasia,
                    TransportadoraTelefone = t.Telefone,
                    ProdutoCodigo = p.Codigo,
                    ProdutoDescricao = p.Descricao,
                    pvi.ProdutoCliente,
                    c.Quantidade,
                    c.Idlote,
                    pvi.PrecoUnitario,
                    Total = c.Quantidade * pvi.PrecoUnitario
                })
                .ToListAsync(ct);

            if (dados.Count == 0)
                return null;

            var cabecalho = dados.First();

            return new ExpedicaoConsumoLoteDto
            {
                NomeCliente = cabecalho.Nome,
                Contato = cabecalho.Contato,
                Municipio = cabecalho.Municipio,
                Uf = cabecalho.Uf,
                ObservacaoOrdemExpedicao = cabecalho.ObservacaoOrdemExpedicao,
                TransportadoraNome = cabecalho.TransportadoraNome,
                TransportadoraTelefone = cabecalho.TransportadoraTelefone,

                Itens = dados.Select(x => new ExpedicaoConsumoLoteItemDto
                {
                    ProdutoCodigo = x.ProdutoCodigo,
                    ProdutoDescricao = x.ProdutoDescricao,
                    ProdutoCliente = x.ProdutoCliente,
                    Quantidade = x.Quantidade,
                    IdLote = x.Idlote,
                    PrecoUnitario = x.PrecoUnitario,
                    Total = x.Total
                }).ToList()
            };
        }
    }
}