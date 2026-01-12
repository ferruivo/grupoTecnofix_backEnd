namespace GrupoTecnofix_Api.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NomeCompleto { get; set; } = null!;
        public string? NomeExibicao { get; set; }
        public string Email { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string SenhaHash { get; set; } = null!;
        public bool Ativo { get; set; } = true;
    }

}
