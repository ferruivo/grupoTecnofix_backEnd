using AutoMapper;
using GrupoTecnofix_Api.Dtos.Produto;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class PrecoVendaProfile : Profile
    {
        public PrecoVendaProfile()
        {
            CreateMap<PrecoVendaDto, Precovendum>();
            CreateMap<Precovendum, PrecoVendaDto>();

            CreateMap<PrecoVendaCreateUpdateDto, Precovendum>();
            CreateMap<Precovendum, PrecoVendaCreateUpdateDto>();
        }
    }
}
