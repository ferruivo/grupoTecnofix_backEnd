namespace GrupoTecnofix_Api.Dtos.Perfil
{
    public class PerfilListItemDto
    {
        public int IdPerfil { get; set; }
        public string Nome { get; set; } = "";
        public string? Descricao { get; set; }
    }

    public class PerfilLookupDto
    {
        public int IdPerfil { get; set; }
        public string Nome { get; set; } = "";
    }

    public class PerfilCreateDto
    {
        public string Nome { get; set; } = "";
        public string? Descricao { get; set; }
    }

    public class PerfilUpdateDto
    {
        public string Nome { get; set; } = "";
        public string? Descricao { get; set; }
    }
}
