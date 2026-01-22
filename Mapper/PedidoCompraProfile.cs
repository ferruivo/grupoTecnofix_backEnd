using AutoMapper;
using GrupoTecnofix_Api.Dtos.PedidoCompra;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mapper
{
    public class PedidoCompraProfile : Profile
    {
        public PedidoCompraProfile()
        {
            CreateMap<PedidoCompraCreateUpdateDto, PedidosCompra>()
                .ForMember(dest => dest.DataEmissao, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DataPedido)))
                .ForMember(dest => dest.IdCondPagamento, opt => opt.MapFrom(src => src.IdCondicaoPagamento))
                .ForMember(dest => dest.ObservacaoCompl, opt => opt.MapFrom(src => src.ObservacaoComplementar))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<PedidoCompraItemCreateUpdateDto, PedidosCompraIten>()
                .ForMember(dest => dest.ValorIpi, opt => opt.MapFrom(src => src.TotalIpi))
                .ForMember(dest => dest.ValorIcms, opt => opt.MapFrom(src => src.TotalIcms))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
