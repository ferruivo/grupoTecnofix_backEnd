using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class TransportadorasService : ITransportadorasService
    {
        private readonly ITransportadorasRepository _repo;
        private readonly IMunicipiosRepository _mun_Repo;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public TransportadorasService(ITransportadorasRepository repo, IMunicipiosRepository mun_Repo, ICurrentUserService currentUser, IMapper mapper)
        {
            _repo = repo;
            _mun_Repo = mun_Repo;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<PagedResult<TransportadoraListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListPagedAsync(page, pageSize, search, ct);
        }

        public async Task<List<TransportadoraListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListAsync(search, ct);
        }

        public async Task<TransportadoraUpdateDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var t = await _repo.GetByIdAsync(id, ct);
            if (t is null) throw new KeyNotFoundException("Transportadora não encontrada.");

            var dto = _mapper.Map<TransportadoraUpdateDto>(t);

            var m = await _mun_Repo.GetByIdAsync(t.IdMunicipio, ct);
            if (m != null)
                dto.Municipio = _mapper.Map<MunicipioDto>(m);

            return dto;
        }

        public async Task<int> CreateAsync(TransportadoraCreateDto dto, CancellationToken ct)
        {
            var transp = _mapper.Map<Transportadora>(dto);
            transp.DataCadastro = DateTime.Now;
            transp.IdUsuarioCadastro = _currentUser.GetUsuarioLogadoId();

            await _repo.AddAsync(transp, ct);
            await _repo.SaveAsync(ct);

            return transp.IdTransportadora;
        }

        public async Task UpdateAsync(int id, TransportadoraUpdateDto dto, CancellationToken ct)
        {
            var t = await _repo.GetByIdAsync(id, ct);
            if (t is null) throw new KeyNotFoundException("Transportadora não encontrada.");

            _mapper.Map(dto, t);
            t.DataAlteracao = DateTime.Now;
            t.IdUsuarioAlteracao = _currentUser.GetUsuarioLogadoId();

            await _repo.SaveAsync(ct);
        }

        public async Task<byte[]> ExportListToExcelAsync(string? search, CancellationToken ct)
        {
            var list = await _repo.GetListExcelAsync(search, ct);
            // use helper to export
            return await Task.Run(() => Helpers.ExcelExporter.ExportToExcel(list, "transportadoras"), ct);
        }
    }
}
