namespace GrupoTecnofix_Api.Dtos.Usuario
{
    public class UsuarioCreateDto
    {
        public string NomeCompleto { get; set; } = "";
        public string NomeExibicao { get; set; } = "";
        public string Login { get; set; } = "";
        public string Email { get; set; } = "";
        public bool Ativo { get; set; } = true;
    }
}
