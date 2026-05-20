using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.ParametroVenda;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class ParametroVendaService : IParametroVendaService
    {

        private readonly IPedidoVendaRepository _repo;
        private readonly IPedidoCompraRepository _comprasRepo;
        private readonly IRecebimentoRepository _recebimentoRepo;


        public ParametroVendaService(IPedidoVendaRepository repo, IPedidoCompraRepository comprasRepo, IRecebimentoRepository recebimentoRepo)
        {
            _repo = repo;
            _comprasRepo = comprasRepo;
            _recebimentoRepo = recebimentoRepo;
        }

        public async Task<ParametroVendaDto?> GetAsync(long idProduto, long? idCliente, CancellationToken ct)
        {
            ParametroVendaDto? dto = new ParametroVendaDto();
            dto.PedidosVenda = await _repo.GetDisponiveisAsync(idProduto, idCliente, ct);
            dto.PedidosCompra = await _comprasRepo.GetDisponiveisAsync(idProduto, ct);
            dto.Lotes = await _recebimentoRepo.GetLotesByIdProdutoAsync(idProduto, ct);
            return dto;
        }
    }
}
