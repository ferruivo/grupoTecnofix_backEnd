using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Data.Repositories;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class AlocacaoMaterialsService : IAlocacaoMaterialsService
    {
        private readonly IAlocacaoMaterialsRepository _repo;
        private readonly IRecebimentoRepository _recebimentoRepo;

        public AlocacaoMaterialsService(IAlocacaoMaterialsRepository repo, IRecebimentoRepository recebimentoRepo)
        {
            _repo = repo;
            _recebimentoRepo = recebimentoRepo;
        }

        public async Task<PagedResult<AlocacaoMaterialListDto>> GetPagedAsync(int page, int pageSize, int idLote, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            var lista = await _repo.GetListPagedAsync(page, pageSize, idLote, ct);
            lista.TotalItems = Convert.ToInt32(await _recebimentoRepo.CountByLoteAsync(idLote, ct));


            return  lista;
        }

        public async Task<List<AlocacaoMaterialListDto>> SalvarAlocacoesAsync(AlocacaoMaterialSalvarDto dto, CancellationToken ct)
        {
            if (dto.IdLote <= 0)
                throw new ArgumentException("Lote inválido.");

            if (dto.Alocacoes == null || dto.Alocacoes.Count == 0)
                throw new ArgumentException("Informe ao menos uma alocação.");

            var atuais = await _repo.GetByLoteAsync(dto.IdLote, ct);

            foreach (var item in dto.Alocacoes)
            {
                var existente = atuais.FirstOrDefault(x => x.Etq == item.Etq);

                if (existente == null)
                {
                    var nova = new AlocacaoMaterial
                    {
                        IdLote = dto.IdLote,
                        Etq = item.Etq,
                        IdPrateleira = item.IdPrateleira.ToString(),
                        Qtd = item.Qtd,
                        //Reservado = item.Reservado,
                        DataAlocacao = item.DataAlocacao ?? DateTime.Now
                    };

                    await _repo.AddAsync(nova, ct);
                    continue;
                }

                existente.IdPrateleira = item.IdPrateleira.ToString();
                existente.Qtd = item.Qtd;
                //existente.Reservado = item.Reservado;
                existente.DataAlocacao = item.DataAlocacao ?? existente.DataAlocacao ?? DateTime.Now;
            }

            var etiquetasRecebidas = dto.Alocacoes.Select(x => x.Etq).ToHashSet();

            var paraRemover = atuais
                .Where(x => !etiquetasRecebidas.Contains(x.Etq))
                .ToList();

            foreach (var item in paraRemover)
                _repo.Remove(item);

            await _repo.SaveAsync(ct);

            var atualizadas = await _repo.GetByLoteAsync(dto.IdLote, ct);

            return atualizadas
                .OrderBy(x => x.Etq)
                .Select(x => new AlocacaoMaterialListDto
                {
                    IdAlocacaoMaterial = x.IdAlocacaoMaterial,
                    IdLote = x.IdLote,
                    Etq = x.Etq,
                    IdPrateleira = Convert.ToInt32(x.IdPrateleira),
                    Qtd = x.Qtd,
                    Reservado = x.Reservado,
                    DataAlocacao = x.DataAlocacao
                })
                .ToList();
        }
    }
}
