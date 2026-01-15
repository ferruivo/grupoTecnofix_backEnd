using AutoMapper;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class MunicipioProfile : Profile
    {
        public MunicipioProfile() 
        {
            // Entity -> List / Get
            CreateMap<Municipio, MunicipioDto>();
        }
    }
}
