using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.NotaFiscal;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class NotaFiscalService : INotaFiscalService
    {
        private readonly INotaFiscalRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        private readonly INotaFiscalCalculoService _calculoService;

        public NotaFiscalService(
            INotaFiscalRepository repo,
            ICurrentUserService currentUser,
            IMapper mapper,
            INotaFiscalCalculoService calculoService)
        {
            _repo = repo;
            _currentUser = currentUser;
            _mapper = mapper;
            _calculoService = calculoService;
        }

        public async Task<PagedResult<NotaFiscalListDto>> GetPagedAsync(
            int page,
            int pageSize,
            string? search,
            CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListPagedAsync(page, pageSize, search, ct);
        }

        public async Task<NotaFiscal?> GetByIdAsync(
            long idNotaFiscal,
            CancellationToken ct)
        {
            var nota = await _repo.GetByIdAsync(idNotaFiscal, ct);

            if (nota is null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            return nota;
        }

        public async Task<long> CreateAsync(
            NotaFiscalCreateDto dto,
            CancellationToken ct)
        {
            var nota = _mapper.Map<NotaFiscal>(dto);

            nota.Status = string.IsNullOrWhiteSpace(nota.Status)
                ? "DIGITADA"
                : nota.Status;

            nota.DataCadastro = DateTime.Now;

            nota.EnsureCreationAudit(_currentUser);

            await _calculoService.CalcularAsync(nota, ct);

            await _repo.AddAsync(nota, ct);
            await _repo.SaveAsync(ct);

            return nota.IdNotaFiscal;
        }

        public async Task UpdateAsync(
            long idNotaFiscal,
            NotaFiscalUpdateDto dto,
            CancellationToken ct)
        {
            var nota = await _repo.GetByIdForUpdateAsync(idNotaFiscal, ct);

            if (nota is null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            _mapper.Map(dto, nota);

            nota.DataAlteracao = DateTime.Now;
            nota.EnsureUpdateAudit(_currentUser);

            await _calculoService.CalcularAsync(nota, ct);

            await _repo.SaveAsync(ct);
        }

        public async Task DeleteAsync(
            long idNotaFiscal,
            CancellationToken ct)
        {
            var nota = await _repo.GetByIdForUpdateAsync(idNotaFiscal, ct);

            if (nota is null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            await _repo.DeleteAsync(nota, ct);
            await _repo.SaveAsync(ct);
        }

        public async Task AddEventoAsync(
            NotaFiscalEventoCreateDto dto,
            CancellationToken ct)
        {
            var nota = await _repo.GetByIdForUpdateAsync(dto.IdNotaFiscal, ct);

            if (nota is null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            var evento = _mapper.Map<NotaFiscalEvento>(dto);

            evento.DataEvento = DateTime.Now;

            await _repo.AddEventoAsync(evento, ct);

            if (evento.TipoEvento.Equals("CANCELAMENTO", StringComparison.OrdinalIgnoreCase))
            {
                nota.Status = "CANCELADA";
                nota.DataCancelamento = DateTime.Now;
                nota.UsuarioCancelamento = dto.Usuario;
                nota.MotivoCancelamento = dto.Justificativa;
                nota.DataAlteracao = DateTime.Now;

                nota.EnsureUpdateAudit(_currentUser);
            }

            await _repo.SaveAsync(ct);
        }

        public async Task EmitirAsync(
    long idNotaFiscal,
    CancellationToken ct)
        {
            var nota = await _repo.GetByIdForUpdateAsync(idNotaFiscal, ct);

            if (nota is null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            await _calculoService.CalcularAsync(nota, ct);

            nota.Status = "EM_PROCESSAMENTO";
            nota.DataAlteracao = DateTime.Now;
            nota.EnsureUpdateAudit(_currentUser);

            await _repo.SaveAsync(ct);

            // Aqui depois entra:
            // gerar XML
            // assinar XML
            // transmitir SEFAZ
            // atualizar protocolo/status
        }

        public Task<NotaFiscalImportacaoPreviewDto> ImportarItensPreviewAsync(
            IFormFile arquivo,
            CancellationToken ct)
        {
            var result = new NotaFiscalImportacaoPreviewDto();

            // Aqui depois entra a leitura real do Excel.
            // Por enquanto retorna estrutura pronta para o front integrar.

            return Task.FromResult(result);
        }
    }
}