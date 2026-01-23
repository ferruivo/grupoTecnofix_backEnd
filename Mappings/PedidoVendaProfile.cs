using AutoMapper;
using GrupoTecnofix_Api.Dtos.PedidoVenda;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class PedidoVendaProfile : Profile
    {
        public PedidoVendaProfile()
        {
            CreateMap<PedidosVendum, PedidoVendaDto>();
            CreateMap<PedidosVendaIten, PedidoVendaItemDto>();
            CreateMap<PedidoVendaCreateUpdateDto, PedidosVendum>();
            CreateMap<PedidoVendaItemCreateUpdateDto, PedidosVendaIten>();
        }
    }
}