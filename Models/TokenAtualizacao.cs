namespace GrupoTecnofix_Api.Models
{
    public class TokenAtualizacao
    {
        public long IdTokenAtualizacao { get; set; }
        public int IdUsuario { get; set; }
        public string TokenHash { get; set; } = null!;
        public DateTime DataExpiracao { get; set; }
        public DateTime? DataRevogacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public string? IpCriacao { get; set; }
        public string? UserAgentCriacao { get; set; }
    }
}
