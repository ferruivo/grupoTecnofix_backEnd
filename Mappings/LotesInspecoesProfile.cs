using AutoMapper;
using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class LotesInspecoesProfile : Profile
    {
        public LotesInspecoesProfile()
        {
            CreateMap<LoteInspecao, LoteInspecaoDto>();
            CreateMap<LoteInspecaoDto, LoteInspecao>();
        }
    }
}
