namespace GrupoTecnofix_Api.Dtos.Vendedor
{
    public class VendedorUpdateDto
    {
        public int IdUsuario { get; set; }
        public bool Interno { get; set; }
        public bool Externo { get; set; }
        public string Observacao { get; set; } = "";
    }
}
