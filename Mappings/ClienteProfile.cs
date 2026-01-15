using AutoMapper;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            // Entity → DTO (list + edit)
            CreateMap<Cliente, ClienteListDto>();
            CreateMap<Cliente, ClienteDto>();
            CreateMap<ClienteCreateUpdate, Cliente>(); 
        }
    }
}
