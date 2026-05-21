using DocumentFormat.OpenXml.Office2010.Excel;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Dtos.ParametroVenda;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class RecebimentoRepository : IRecebimentoRepository
    {
        private readonly AppDbContext _db;

        public RecebimentoRepository(AppDbContext db) => _db = db;

        public async Task<RecebimentoPedidoDto?> GetPedidoByNumeroAsync(int numero, CancellationToken ct)
        {
            var pedido = await _db.PedidosCompras
                .AsNoTracking()
                .Where(p => p.IdPedidoCompra == numero)
                .Select(p => new RecebimentoPedidoDto
                {
                    IdPedidoCompra = p.IdPedidoCompra,
                    Numero = p.IdPedidoCompra,
                    DataPedido = p.DataEmissao,

                    IdFornecedor = p.IdFornecedor,
                    FornecedorNome = _db.Fornecedores
                        .Where(f => f.IdFornecedor == p.IdFornecedor)
                        .Select(f => f.IdFornecedor + "-" + f.Fantasia)
                        .FirstOrDefault() ?? string.Empty,

                    IdCondicaoPagamento = p.IdCondPagamento ?? 0,
                    CondicaoPagamentoDescricao = p.IdCondPagamento == null
                        ? string.Empty
                        : _db.Condicoespagamentos
                            .Where(c => c.IdCondicoespagamento == p.IdCondPagamento.Value)
                            .Select(c => c.Descricao)
                            .FirstOrDefault() ?? string.Empty,

                    TipoFrete = p.TipoFrete,

                    TotalIpi = p.TotalIpi,
                    TotalIcms = p.TotalIcms,

                    Observacao = p.Observacao,

                    Itens = _db.PedidosCompraItens
                        .AsNoTracking()
                        .Where(i => i.IdPedidoCompra == p.IdPedidoCompra)
                        .OrderBy(i => i.IdPedidoCompraItem)
                        .Select(i => new RecebimentoPedidoItemDto
                        {
                            Id = i.IdPedidoCompraItem,
                            IdPedidoCompraItem = i.Item,
                            IdPedidoCompra = i.IdPedidoCompra,

                            IdProduto = i.IdProduto,

                            ProdutoCodigo = _db.Produtos
                                .Where(prod => prod.IdProduto == i.IdProduto)
                                .Select(prod => prod.Codigo)
                                .FirstOrDefault() ?? string.Empty,

                            ProdutoDescricao = _db.Produtos
                                .Where(prod => prod.IdProduto == i.IdProduto)
                                .Select(prod => prod.Descricao)
                                .FirstOrDefault() ?? string.Empty,

                            Item = i.IdPedidoCompraItem,

                            Quantidade = i.Quantidade,

                            QuantidadeRecebida = _db.Lotes
                                .Where(l =>
                                    l.PedidoCompra == i.IdPedidoCompra &&
                                    l.ItemPedidoCompra == i.Item)
                                .Sum(l => (decimal?)l.Quantidade) ?? 0,

                            QuantidadeDisponivel =
                                i.Quantidade -
                                (
                                    _db.Lotes
                                        .Where(l =>
                                            l.PedidoCompra == i.IdPedidoCompra &&
                                            l.ItemPedidoCompra == i.Item)
                                        .Sum(l => (decimal?)l.Quantidade) ?? 0
                                ),

                            PrecoUnitario = i.PrecoUnitario,

                            DataEntrega = i.DataEntrega.HasValue
                                ? DateOnly.FromDateTime(i.DataEntrega.Value)
                                : null,

                            Fechado =
                                (
                                    _db.Lotes
                                        .Where(l =>
                                            l.PedidoCompra == i.IdPedidoCompra &&
                                            l.ItemPedidoCompra == i.Item)
                                        .Sum(l => (decimal?)l.Quantidade) ?? 0
                                ) >= i.Quantidade
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (pedido is null)
                return null;

            pedido.Fechado = pedido.Itens.Count > 0 && pedido.Itens.All(x => x.Fechado);

            return pedido;
        }

        public async Task<RecebimentoLoteDto> CriarLoteAsync(RecebimentoLoteCreateDto dto, int idUsuario, CancellationToken ct)
        {
            if (dto.IdPedidoCompra <= 0)
                throw new InvalidOperationException("Pedido de compra inválido.");

            if (dto.IdPedidoCompraItem <= 0)
                throw new InvalidOperationException("Item do pedido de compra inválido.");

            if (dto.QuantidadeRecebida <= 0)
                throw new InvalidOperationException("A quantidade recebida deve ser maior que zero.");

            var item = await _db.PedidosCompraItens
                .AsNoTracking()
                .FirstOrDefaultAsync(i =>
                    i.IdPedidoCompra == dto.IdPedidoCompra &&
                    i.Item == dto.IdPedidoCompraItem,
                    ct);

            if (item is null)
                throw new KeyNotFoundException("Item do pedido de compra não encontrado.");

            var quantidadeJaRecebida = await _db.Lotes
                .AsNoTracking()
                .Where(l =>
                    l.PedidoCompra == dto.IdPedidoCompra &&
                    l.ItemPedidoCompra == dto.IdPedidoCompraItem)
                .SumAsync(l => (decimal?)l.Quantidade, ct) ?? 0;

            var quantidadeDisponivel = item.Quantidade - quantidadeJaRecebida;

            if (quantidadeDisponivel <= 0)
                throw new InvalidOperationException("Este item já foi totalmente recebido.");

            if (dto.QuantidadeRecebida > quantidadeDisponivel)
                throw new InvalidOperationException(
                    $"Quantidade recebida maior que a disponível. Disponível: {quantidadeDisponivel:N3}.");

            var nfCompra = 0;

            if (!string.IsNullOrWhiteSpace(dto.Nf))
            {
                if (!int.TryParse(dto.Nf.Trim(), out nfCompra))
                    throw new InvalidOperationException("NF deve conter apenas números.");
            }

            var lote = new Lote
            {
                PedidoCompra = dto.IdPedidoCompra,
                ItemPedidoCompra = dto.IdPedidoCompraItem,
                NfCompra = nfCompra,
                IdProduto = item.IdProduto,

                Quantidade = dto.QuantidadeRecebida,
                QtdUtilizada = 0,

                Qpecas = dto.QuantidadePecas.HasValue ? "S" : "N",
                ObsQpecas = dto.QuantidadePecas.HasValue
                    ? dto.QuantidadePecas.Value.ToString("N3")
                    : null,

                Aspecto = dto.AspectoVisualOk ? "S" : "N",
                ObsAspecto = dto.AspectoVisualOk ? null : "",

                Embalagem = dto.EmbalagemOk ? "S" : "N",
                ObsEmbalagem = dto.EmbalagemOk ? null : "",

                Status = "I",
                DataEntrada = dto.Data,

                Obs = dto.Observacao,
                Certificado = dto.Certificado
            };

            _db.Lotes.Add(lote);

            await _db.SaveChangesAsync(ct);

            return new RecebimentoLoteDto
            {
                Id = lote.IdLote,
                IdLote = lote.IdLote,
                IdPedidoCompra = lote.PedidoCompra,
                IdPedidoCompraItem = lote.ItemPedidoCompra,
                Data = lote.DataEntrada,
                Nf = lote.NfCompra == 0 ? null : lote.NfCompra.ToString(),
                Certificado = lote.Certificado,
                QuantidadeRecebida = lote.Quantidade,
                Observacao = lote.Obs
            };
        }

        public async Task<List<RecebimentoLoteDto>> GetLotesByPedidoAsync(int idPedidoCompra, CancellationToken ct)
        {
            return await _db.Lotes
                .AsNoTracking()
                .Where(l => l.PedidoCompra == idPedidoCompra)
                .OrderByDescending(l => l.IdLote)
                .Select(l => new RecebimentoLoteDto
                {
                    Id = l.IdLote,
                    IdLote = l.IdLote,

                    IdPedidoCompra = l.PedidoCompra,
                    IdPedidoCompraItem = l.ItemPedidoCompra,

                    Item = l.ItemPedidoCompra,

                    ProdutoCodigo = _db.Produtos
                        .Where(p => p.IdProduto == l.IdProduto)
                        .Select(p => p.Codigo)
                        .FirstOrDefault(),

                    ProdutoDescricao = _db.Produtos
                        .Where(p => p.IdProduto == l.IdProduto)
                        .Select(p => p.Descricao)
                        .FirstOrDefault(),

                    Data = l.DataEntrada,

                    Nf = l.NfCompra == 0 ? null : l.NfCompra.ToString(),
                    Certificado = l.Certificado,

                    QuantidadeRecebida = l.Quantidade,
                    Status = l.Status,
                    Observacao = l.Obs
                })
                .ToListAsync(ct);
        }

        public async Task<List<LoteDisponivelDto>> GetLotesDisponiveisAsync(int idProduto, CancellationToken ct)
        {
            var items = await (
                from l in _db.Lotes.AsNoTracking()
                where l.IdProduto == idProduto
                orderby l.IdLote descending
                select new LoteDisponivelDto
                {
                    IdLote = l.IdLote,
                    NumeroLote = l.IdLote.ToString(),

                    IdFornecedor = 0,
                    FornecedorCodigo = string.Empty,
                    FornecedorNome = string.Empty,

                    Status = l.Status,

                    Saldo = l.Quantidade - (l.QtdUtilizada ?? 0),

                    Reservado = 0,

                    Disponivel = (l.Quantidade - (l.QtdUtilizada ?? 0)),

                    DataEntrada = l.DataEntrada,
                    DataValidade = null
                })
                .Where(x => x.Disponivel > 0)
                .ToListAsync(ct);

            return items;
        }

        public async Task<List<LoteParametroVendaDto>> GetLotesByIdProdutoAsync(long idProduto, CancellationToken ct)
        {
            try
            {
                var dados = await
            (
                from l in _db.Lotes.AsNoTracking()

                join pci in _db.PedidosCompraItens.AsNoTracking()
                    on new
                    {
                        IdPedidoCompra = l.PedidoCompra,
                        Item = l.ItemPedidoCompra
                    }
                    equals new
                    {
                        IdPedidoCompra = pci.IdPedidoCompra,
                        Item = pci.Item
                    }

                join pc in _db.PedidosCompras.AsNoTracking()
                    on pci.IdPedidoCompra equals pc.IdPedidoCompra

                join f in _db.Fornecedores.AsNoTracking()
                    on pc.IdFornecedor equals f.IdFornecedor

                where l.Quantidade > l.QtdUtilizada
                      && l.IdProduto == idProduto

                select new
                {
                    l.IdLote,
                    l.DataEntrada,
                    f.Fantasia,
                    pci.PrecoUnitario,
                    Disponivel = l.Quantidade - l.QtdUtilizada,
                    l.Status
                }
            ).ToListAsync(ct);

                return dados.Select(x => new LoteParametroVendaDto
                {
                    Lote = x.IdLote,
                    Entrada = x.DataEntrada.ToString(),
                    Fornecedor = x.Fantasia,
                    Preco = x.PrecoUnitario,
                    Disponivel = x.Disponivel,
                    Status = x.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return null;
            }
        }

        public async Task<EtiquetaProdutoDto> GetLoteEtiquetaByIdAsync(long idLote, CancellationToken ct)
        {
            try
            {
                return await (
                    from l in _db.Lotes.AsNoTracking()
                    join p in _db.Produtos.AsNoTracking()
                    on l.IdProduto equals p.IdProduto
                    where l.IdLote == idLote


                    select new EtiquetaProdutoDto
                    {
                        ProdutoCodigo = p.Codigo,
                        ProdutoDescricao = p.Descricao,
                        QuantidadeTotal = l.Quantidade,
                        Fator = p.FatorEmbalagem,
                        Lote = l.IdLote.ToString()
                    }
                ).FirstOrDefaultAsync(ct);


            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return null;
            }
        }

        public async Task<decimal> CountByLoteAsync(long idLote, CancellationToken ct)
        {
            try
            {
                return await _db.Lotes.Where(x => x.IdLote == idLote).Select(x => x.Quantidade).FirstOrDefaultAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}