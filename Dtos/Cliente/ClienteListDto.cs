using GrupoTecnofix_Api.Dtos.Municipios;

namespace GrupoTecnofix_Api.Dtos.Cliente
{
    public class ClienteListDto
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = "";
        public string Fantasia { get; set; } = "";
        public string Contato { get; set; } = "";
        public string? Cpf { get; set; } = "";
        public string? Cnpj { get; set; } = "";

        public MunicipioDto? Municipio { get; set; }
    }
}
