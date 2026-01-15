using GrupoTecnofix_Api.Dtos.Municipios;

namespace GrupoTecnofix_Api.Dtos.Transportadoras
{
    public class TransportadoraUpdateDto
    {
        public int IdTransportadora { get; set; }
        public string Cnpj { get; set; } = null!;
        public string? InscricaoEstadual { get; set; }
        public string RazaoSocial { get; set; } = null!;
        public string Fantasia { get; set; } = null!;
        public string Contato { get; set; } = null!;
        public string? Telefone { get; set; }
        public string Cep { get; set; } = null!;
        public string Endereco { get; set; } = null!;
        public string Bairro { get; set; } = null!;
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public int IdMunicipio { get; set; }
        public MunicipioDto? Municipio { get; set; }
    }
}
