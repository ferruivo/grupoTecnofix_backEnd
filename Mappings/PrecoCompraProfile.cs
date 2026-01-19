using AutoMapper;
using GrupoTecnofix_Api.Dtos.Produto;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.OUT.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class PrecoCompraProfile : Profile
    {
        public PrecoCompraProfile()
        {
            CreateMap<PrecoCompraDto, Precocompra>();
            CreateMap<Precocompra, PrecoCompraDto>();

            CreateMap<PrecoCompraCreateUpdateDto, Precocompra>();
            CreateMap<Precocompra, PrecoCompraCreateUpdateDto>();
        }
    }
}
