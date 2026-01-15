using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Perfis;

namespace GrupoTecnofix_Api.Dtos.Transportadoras
{
    public class TransportadoraListDto
    {
        public int IdTransportadora { get; set; }
        public string CNPJ { get; set; }
        public string Fantasia { get; set; } = "";
        public string RazaoSocial { get; set; } = "";
        public string Contato { get; set; } = "";
        public MunicipioDto Municipio { get; set; } = new(); 
    }
}
