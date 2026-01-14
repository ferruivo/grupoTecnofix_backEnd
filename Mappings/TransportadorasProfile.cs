using AutoMapper;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class TransportadorasProfile : Profile
    {
        public TransportadorasProfile()
        {
            // Create -> Entity
            CreateMap<TransportadoraCreateDto, Transportadora>();

            // Update -> Entity (ignorando campos que não podem ser alterados)
            CreateMap<TransportadoraUpdateDto, Transportadora>()
                .ForMember(d => d.IdTransportadora, opt => opt.Ignore()); // ajuste nome do Id conforme sua model
        }
    }
}
