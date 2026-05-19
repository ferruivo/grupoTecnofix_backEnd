using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class LotesInspecoesService : ILotesInspecoesService
    {
        private readonly ILotesInspecoesRepository _repo;
        private readonly IProdutosEspecificacoesRepository _prodEspRepo;
        private readonly IEspecificacoesRepository _espRepo;
        private readonly IMapper _mapper;

        public LotesInspecoesService(ILotesInspecoesRepository repo, IProdutosEspecificacoesRepository prodEspRepo, IEspecificacoesRepository espRepo, IMapper mapper)
        {
            _repo = repo;
            _prodEspRepo = prodEspRepo;
            _espRepo = espRepo;
            _mapper = mapper;
        }

        public async Task CreateOrUpdateBatchAsync(int idLote, List<LoteInspecaoDto> items, string? statusLote, CancellationToken ct)
        {
            if (items == null || items.Count == 0)
                return;

            foreach (var dto in items)
            {
                var existing = await _repo.GetByLoteEspecificacaoAsync(idLote, dto.IdEspecificacao, ct);
                if (existing is null)
                {
                    var ent = _mapper.Map<LoteInspecao>(dto);
                    ent.IdLote = idLote;
                    ent.DataCadastro = DateTime.Now;
                    await _repo.AddAsync(ent, ct);
                }
                else
                {
                    existing.Minimo = dto.Minimo;
                    existing.Maximo = dto.Maximo;
                    existing.Aproximado = dto.Aproximado;
                    existing.Observacao = dto.Observacao;
                }
            }

            // atualização do status do lote é feita no controller (responsabilidade separada)
            await _repo.SaveAsync(ct);
        }

        public async Task<List<LoteInspecaoDto>> GetByLoteAsync(int idLote, CancellationToken ct)
        {
            var items = await _repo.GetByLoteAsync(idLote, ct);
            return items.Select(x => _mapper.Map<LoteInspecaoDto>(x)).ToList();
        }

        public async Task<LoteInspecaoDto> GetByLoteEspecificacaoAsync(int idLote, int idEspecificacao, CancellationToken ct)
        {
            var item = await _repo.GetByLoteEspecificacaoAsync(idLote, idEspecificacao, ct);
            if (item is null) throw new KeyNotFoundException("Inspeção não encontrada.");
            return _mapper.Map<LoteInspecaoDto>(item);
        }

        public async Task<int> CreateOrUpdateAsync(LoteInspecaoDto dto, CancellationToken ct)
        {
            // ensure especificacao belongs to produto of lote: this check is left minimal (assumes caller ensures lote-product mapping)
            var existing = await _repo.GetByLoteEspecificacaoAsync(dto.IdLote, dto.IdEspecificacao, ct);
            if (existing is null)
            {
                var ent = _mapper.Map<LoteInspecao>(dto);
                ent.DataCadastro = DateTime.Now;
                await _repo.AddAsync(ent, ct);
                await _repo.SaveAsync(ct);
                return ent.IdLoteInspecao;
            }
            else
            {
                existing.Minimo = dto.Minimo;
                existing.Maximo = dto.Maximo;
                existing.Aproximado = dto.Aproximado;
                existing.Observacao = dto.Observacao;
                await _repo.SaveAsync(ct);
                return existing.IdLoteInspecao;
            }
        }

        public async Task DeleteAsync(int idLoteInspecao, CancellationToken ct)
        {
            await _repo.DeleteAsync(idLoteInspecao, ct);
            await _repo.SaveAsync(ct);
        }
    }
}
