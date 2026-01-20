namespace GrupoTecnofix_Api.Dtos.Usuario
{
    public class UsuarioUpdateDto
    {
        public string NomeCompleto { get; set; } = "";
        public string NomeExibicao { get; set; } = "";
        public string Email { get; set; } = "";
        public string SenhaHash { get; set; } = "";
        public bool Ativo { get; set; } = true;
    }
}
