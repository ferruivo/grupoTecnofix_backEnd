using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class FornecedoresService : IFornecedoresService
    {
        private readonly IFornecedoresRepository _repo;
        private readonly IMunicipiosRepository _mun_repo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public FornecedoresService(IFornecedoresRepository repo, IMunicipiosRepository mun_repo, IMapper mapper, ICurrentUserService currentUser)
        {
            _repo = repo;
            _mun_repo = mun_repo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<FornecedorListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListPagedAsync(page, pageSize, search, ct);
        }

        public async Task<List<FornecedorListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListAsync(search, ct);
        }

        public async Task<FornecedorDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var f = await _repo.GetByIdAsync(id, ct);
            if (f is null) throw new KeyNotFoundException("Fornecedor não encontrado.");

            var dto = _mapper.Map<FornecedorDto>(f);

            dto.Municipio = _mapper.Map<MunicipioDto>(await _mun_repo.GetByIdAsync(f.IdMunicipio, ct));

            return dto;
        }

        public async Task<int> CreateAsync(FornecedorCreateUpdateDto dto, CancellationToken ct)
        {
            var forn = _mapper.Map<Fornecedore>(dto);

            forn.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(forn, ct);
            await _repo.SaveAsync(ct);

            return forn.IdFornecedor;
        }

        public async Task UpdateAsync(int id, FornecedorCreateUpdateDto dto, CancellationToken ct)
        {
            var f = await _repo.GetByIdAsync(id, ct);
            if (f is null) throw new KeyNotFoundException("Fornecedor não encontrado.");

            _mapper.Map(dto, f);

            f.EnsureUpdateAudit(_currentUser);

            await _repo.SaveAsync(ct);
        }

        public async Task<byte[]> ExportListToExcelAsync(string? search, CancellationToken ct)
        {
            var list = await _repo.GetListPagedAsync(1, 1000, search, ct);
            // use helper to export
            return await Task.Run(() => Helpers.ExcelExporter.ExportToExcel(list.Items, "Fornecedores"), ct);
        }
    }
}
