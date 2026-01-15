using AutoMapper;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Mappings
{
    public class TipoDocumentoProfile : Profile
    {
        public TipoDocumentoProfile()
        {
            CreateMap<Tipodocumento, TipoDocumentoDto>();
            CreateMap<TipoDocumentoDto, Tipodocumento>();
        }
    }
}
