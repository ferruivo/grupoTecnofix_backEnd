
using GrupoTecnofix_Api.Dtos.Usuario;

namespace GrupoTecnofix_Api.Dtos.Vendedor
{
    public class VendedorDto
    {
        public int IdVendedor { get; set; }
        public int IdUsuario { get; set; }
        public UsuarioDto Usuario { get; set; }
        public bool Interno { get; set; }
        public bool Externo { get; set; }
        public string Observacao { get; set; } = "";
    }
}
