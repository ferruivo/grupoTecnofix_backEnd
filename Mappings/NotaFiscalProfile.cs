using AutoMapper;
using GrupoTecnofix_Api.Dtos.NotaFiscal;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class NotaFiscalProfile : Profile
    {
        public NotaFiscalProfile()
        {
            // Create maps for create DTO -> entity
            CreateMap<NotaFiscalCreateDto, NotaFiscal>()
                .ForMember(dest => dest.NotaFiscalItems, opt => opt.MapFrom(src => src.Itens));

            CreateMap<NotaFiscalItemCreateDto, NotaFiscalItem>()
                .ForMember(dest => dest.NotaFiscalItemTributos, opt => opt.MapFrom(src => src.Tributos));

            CreateMap<NotaFiscalItemTributoCreateDto, NotaFiscalItemTributo>();

            // Update maps
            CreateMap<NotaFiscalUpdateDto, NotaFiscal>()
                .ForMember(dest => dest.NotaFiscalItems, opt => opt.MapFrom(src => src.Itens))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<NotaFiscalItemUpdateDto, NotaFiscalItem>()
                .ForMember(dest => dest.NotaFiscalItemTributos, opt => opt.MapFrom(src => src.Tributos))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
