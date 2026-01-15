using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IEmpresaRepository _repo;
        private readonly IMunicipiosRepository _mun_Repo;
        private readonly IMapper _mapper;

        public EmpresaService(ICurrentUserService currentUser, IEmpresaRepository repo, IMunicipiosRepository mun_Rep, IMapper mapper)
        {
            _currentUser = currentUser;
            _repo = repo;
            _mun_Repo = mun_Rep;
            _mapper = mapper;
        }

        public async Task<EmpresaDto> GetAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetAsync(search, ct);
        }

        public async Task<EmpresaDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var e = await _repo.GetByIdAsync(id, ct);
            if (e is null) throw new KeyNotFoundException("Empresa não encontrada.");

            var dto = _mapper.Map<EmpresaDto>(e);

            var m = await _mun_Repo.GetByIdAsync(e.IdMunicipio, ct);
            if (m != null)
                dto.Municipio = _mapper.Map<MunicipioDto>(m);

            return dto;
        }

        public async Task UpdateAsync(int id, EmpresaUpdateDto dto, CancellationToken ct)
        {
            var e = await _repo.GetByIdAsync(id, ct);
            if (e is null) throw new KeyNotFoundException("Empresa não encontrada.");

            try
            {
                _mapper.Map(dto, e);
            }
            catch(Exception ex)
            {
                var x = ex.Message;
            }
            

            await _repo.SaveAsync(ct);
        }
    }
}
