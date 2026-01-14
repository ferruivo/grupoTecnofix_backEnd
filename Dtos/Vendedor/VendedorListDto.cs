using GrupoTecnofix_Api.Dtos.Perfis;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Dtos.Vendedor
{
    public class VendedorListDto
    {
        public int IdVendedor { get; set; }
        public UsuarioDto Usuario { get; set; }
        public bool Interno { get; set; }
        public bool Externo { get; set; }
        public string Observacao { get; set; } = "";
    }
}
