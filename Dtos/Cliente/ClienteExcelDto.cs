using GrupoTecnofix_Api.Dtos.Municipios;

namespace GrupoTecnofix_Api.Dtos.Cliente
{
    public class ClienteExcelDto
    {
        public string Nome { get; set; } = "";
        public string Fantasia { get; set; } = "";
        public string Contato { get; set; } = "";
        public string? Cpf { get; set; } = "";
        public string? Cnpj { get; set; } = "";
        public string? Municipio { get; set; }
        public string? UF { get; set; }
        public string? OrigemCadastro { get; set; }
    }
}
