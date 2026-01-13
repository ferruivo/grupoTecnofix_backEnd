using GrupoTecnofix_Api.Dtos.Perfis;

namespace GrupoTecnofix_Api.Dtos.Usuario
{
    public class UsuarioListDto
    {
        public int IdUsuario { get; set; }
        public string NomeCompleto { get; set; } = "";
        public string Login { get; set; } = "";
        public string Email { get; set; } = "";
        public bool Ativo { get; set; }

        public List<PerfilDto> Perfis { get; set; } = new();
    }

}
