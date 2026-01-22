using GrupoTecnofix_Api.Dtos.Municipios;

namespace GrupoTecnofix_Api.Dtos.Transportadora
{
    public class TransportadoraExcelDto
    {
        public string Fantasia { get; set; } = "";
        public string RazaoSocial { get; set; } = "";
        public string CNPJ { get; set; }
        public string Contato { get; set; } = "";
        public string Muicipio { get; set; } = "";
        public string UF { get; set; } = "";
    }
}
