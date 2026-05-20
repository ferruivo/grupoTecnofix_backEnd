using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Dtos.PedidoVenda;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class LiberacaoPedidoService : ILiberacaoPedidoService
    {
        private readonly IPedidoVendaRepository _repo;
        private readonly IRecebimentoRepository _recebimentoRepo;
        private readonly ICurrentUserService _currentUser;

        public LiberacaoPedidoService(
            IPedidoVendaRepository repo,
            IRecebimentoRepository recebimentoRepo,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _recebimentoRepo = recebimentoRepo;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<PedidoVendaPendenteItemDto>> GetPendentesLiberacaoAsync(
            int idCliente,
            int page,
            int pageSize,
            CancellationToken ct)
        {
            if (idCliente <= 0)
                throw new ArgumentException("Cliente inválido.");

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetPendentesLiberacaoAsync(idCliente, page, pageSize, ct);
        }

        public async Task<List<LoteDisponivelDto>> GetLotesDisponiveisAsync(
            int idProduto,
            CancellationToken ct)
        {
            if (idProduto <= 0)
                throw new ArgumentException("Produto inválido.");

            return await _recebimentoRepo.GetLotesDisponiveisAsync(idProduto, ct);
        }

        public async Task<LiberarPendenteResponseDto> LiberarPendenteAsync(LiberarPendenteRequestDto dto,CancellationToken ct)
        {
            if (dto is null)
                throw new ArgumentException("Dados da liberação não informados.");

            if (dto.IdPedidoVenda <= 0)
                throw new ArgumentException("Pedido de venda inválido.");

            if (dto.Item <= 0)
                throw new ArgumentException("Item inválido.");

            if (dto.IdProduto <= 0)
                throw new ArgumentException("Produto inválido.");

            if (dto.Alocacoes is null || dto.Alocacoes.Count == 0)
                throw new ArgumentException("Informe ao menos uma alocação.");

            if (dto.Alocacoes.Any(x => x.IdLote <= 0 || x.Quantidade <= 0))
                throw new ArgumentException("Todas as alocações devem possuir lote e quantidade maior que zero.");

            dto.Alocacoes = dto.Alocacoes
                .GroupBy(x => x.IdLote)
                .Select(g => new LiberarPendenteAlocacaoRequestDto
                {
                    IdLote = g.Key,
                    Quantidade = g.Sum(x => x.Quantidade)
                })
                .ToList();

            return await _repo.LiberarPendenteAsync(dto,_currentUser.GetUsuarioLogadoId(),ct);
        }

        public async Task<List<LiberacaoAlocacoesDto>> GetAlocacoesLiberadasAsync(
    int idPedidoVenda,
    int? item,
    CancellationToken ct)
        {
            if (idPedidoVenda <= 0)
                throw new ArgumentException("Pedido de venda inválido.");

            return await _repo.GetAlocacoesLiberadasAsync(idPedidoVenda, item, ct);
        }

        public async Task<EstornarLiberacaoResponseDto> EstornarLiberacaoAsync(
            EstornarLiberacaoRequestDto dto,
            CancellationToken ct)
        {
            if (dto is null)
                throw new ArgumentException("Dados do estorno não informados.");

            if (dto.IdPedidoVenda <= 0)
                throw new ArgumentException("Pedido de venda inválido.");

            if (dto.Item <= 0)
                throw new ArgumentException("Item inválido.");

            if (dto.IdLote <= 0)
                throw new ArgumentException("Lote inválido.");

            if (dto.Quantidade.HasValue && dto.Quantidade.Value <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            return await _repo.EstornarLiberacaoAsync(dto, ct);
        }

        public Task<ExpedicaoConsumoLoteDto> GetExpedicaoConsumoLoteAsync(int idCliente, CancellationToken ct)
        {
            return _repo.GetExpedicaoConsumoLoteAsync(idCliente, ct);
        }
    }
}