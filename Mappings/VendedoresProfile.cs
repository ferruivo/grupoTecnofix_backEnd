using AutoMapper;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class VendedoresProfile : Profile
    {
        public VendedoresProfile()
        {
            // DTO -> Entidade (para Create/Update)
            CreateMap<VendedorCreateDto, Vendedore>();
            CreateMap<Vendedore, VendedorDto>();
            CreateMap<VendedorDto, Vendedore>();

            CreateMap<VendedorUpdateDto, Vendedore>()
                // id do vendedor vem da rota, não do body
                .ForMember(d => d.IdVendedor, opt => opt.Ignore())
                // se IdUsuario é imutável no update, ignore:
                .ForMember(d => d.IdUsuario, opt => opt.Ignore());
            
            // Entity -> List / Get
            CreateMap<Vendedore, VendedorDto>();
        }
    }
}
