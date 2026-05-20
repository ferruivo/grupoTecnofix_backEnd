using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class ProdutosEspecificacoesService : IProdutosEspecificacoesService
    {
        private readonly IProdutosEspecificacoesRepository _repo;
        private readonly IEspecificacoesRepository _espRepo;
        private readonly ICurrentUserService _currentUser;

        public ProdutosEspecificacoesService(IProdutosEspecificacoesRepository repo, IEspecificacoesRepository espRepo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _espRepo = espRepo;
            _currentUser = currentUser;
        }

        public async Task<List<EspecificacaoListDto>> GetByProdutoAsync(int idProduto, CancellationToken ct)
        {
            return await _repo.GetByProdutoAsync(idProduto, ct);
        }

        public async Task AddAsync(int idProduto, ProdutoEspecificacaoCreateUpdateDto dto, CancellationToken ct)
        {
            var esp = await _espRepo.GetByIdAsync(dto.IdEspecificacao, ct);
            if (esp is null) throw new KeyNotFoundException("Especificação não encontrada.");

            var pe = new ProdutoEspecificacao
            {
                IdProduto = idProduto,
                IdEspecificacao = dto.IdEspecificacao,
                Minimo = dto.Minimo,
                Maximo = dto.Maximo,
                Aproximado = dto.Aproximado,
                Observacao = dto.Observacao
            };

            await _repo.AddAsync(pe, ct);
            await _repo.SaveAsync(ct);
        }

        public async Task UpdateAsync(int idProduto, int idEspecificacao, ProdutoEspecificacaoCreateUpdateDto dto, CancellationToken ct)
        {
            var existing = await _repo.GetByProdutoEspecificacaoAsync(idProduto, idEspecificacao, ct);
            if (existing is null) throw new KeyNotFoundException("Associação não encontrada.");

            existing.Minimo = dto.Minimo;
            existing.Maximo = dto.Maximo;
            existing.Aproximado = dto.Aproximado;
            existing.Observacao = dto.Observacao;

            await _repo.UpdateAsync(existing, ct);
            await _repo.SaveAsync(ct);
        }

        public async Task DeleteAsync(int idProduto, int idEspecificacao, CancellationToken ct)
        {
            await _repo.DeleteAsync(idProduto, idEspecificacao, ct);
            await _repo.SaveAsync(ct);
        }
    }
}
