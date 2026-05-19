using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class RecebimentoService : IRecebimentoService
    {
        private readonly IRecebimentoRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<RecebimentoService> _logger;   
        public RecebimentoService(IRecebimentoRepository repo, ICurrentUserService currentUser, ILogger<RecebimentoService> logger)
        {
            _repo = repo;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<RecebimentoPedidoDto?> GetPedidoByNumeroAsync(
            int numero,
            CancellationToken ct)
        {
            return await _repo.GetPedidoByNumeroAsync(numero, ct);
        }

        public async Task<RecebimentoLoteDto> CriarLoteAsync(
            RecebimentoLoteCreateDto dto,
            CancellationToken ct)
        {
            var idUsuario = _currentUser.GetUsuarioLogadoId();

            return await _repo.CriarLoteAsync(dto, idUsuario, ct);
        }

        public async Task<List<RecebimentoLoteDto>> GetLotesByPedidoAsync(
    int idPedidoCompra,
    CancellationToken ct)
        {
            return await _repo.GetLotesByPedidoAsync(idPedidoCompra, ct);
        }
    }
}
