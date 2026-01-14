using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Usuario;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class MunicipiosService : IMunicipiosService
    {
        private readonly IMunicipiosRepository _repo;
        private readonly IMapper _mapper;

        public MunicipiosService(IMunicipiosRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<MunicipioDto>> GetListAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListAsync(search, ct);
        }
    }
}
