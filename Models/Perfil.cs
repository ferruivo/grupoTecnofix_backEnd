namespace GrupoTecnofix_Api.Models
{
    public class Perfil
    {
        public int IdPerfil { get; set; }
        public string Nome { get; set; } = null!;
    }

    public class Permissao
    {
        public int IdPermissao { get; set; }
        public string Codigo { get; set; } = null!;
    }

    public class UsuarioPerfil
    {
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
    }

    public class PerfilPermissao
    {
        public int IdPerfil { get; set; }
        public int IdPermissao { get; set; }
    }
}
