using AutoMapper;
using GrupoTecnofix_Api.Dtos.Perfil;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class PerfisProfile : Profile
    {
        public PerfisProfile()
        {
            CreateMap<PerfilCreateDto, Perfi>();
            CreateMap<PerfilUpdateDto, Perfi>()
                .ForMember(d => d.IdPerfil, opt => opt.Ignore());
        }
    }
}
