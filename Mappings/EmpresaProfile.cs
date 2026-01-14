using AutoMapper;
using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class EmpresaProfile : Profile
    {
        public EmpresaProfile()
        {
            CreateMap<EmpresaUpdateDto, Empresa>()
                // id do vendedor vem da rota, não do body
                .ForMember(d => d.IdEmpresa, opt => opt.Ignore());
        }
    }
}
