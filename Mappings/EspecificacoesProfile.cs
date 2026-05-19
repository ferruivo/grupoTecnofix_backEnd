using AutoMapper;
using GrupoTecnofix_Api.Dtos.Especificacao;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class EspecificacoesProfile : Profile
    {
        public EspecificacoesProfile()
        {
            CreateMap<EspecificacaoDto, Especificacao>();
            CreateMap<Especificacao, EspecificacaoDto>();
            CreateMap<Especificacao, EspecificacaoListDto>();
        }
    }
}
