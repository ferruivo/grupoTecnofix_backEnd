using AutoMapper;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrupoTecnofix_Api.Mappings
{
    public class UsuariosProfile : Profile
    {
        public UsuariosProfile()
        {
            CreateMap<UsuarioCreateDto, Usuario>();
            CreateMap<UsuarioUpdateDto, Usuario>()
                .ForMember(d => d.IdUsuario, opt => opt.Ignore())
                .ForMember(d => d.Login, opt => opt.Ignore());

            // Entity -> List / Get
            CreateMap<Usuario, UsuarioDto>();

            CreateMap<UsuarioDto, UsuarioUpdateDto>();
            CreateMap<UsuarioUpdateDto, UsuarioDto>();
        }
    }
}
