namespace GrupoTecnofix_Api.Dtos.Perfil
{
    public class PermissaoDto
    {
        public int IdPermissao { get; set; }
        public string Descricao { get; set; } = "";
        public string Codigo { get; set; } = ""; 
        public bool? Ativo { get; set; }
    }
}
