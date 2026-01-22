namespace GrupoTecnofix_Api.Dtos.Fornecedor
{
    public class FornecedorExcelDto
    {
        public string? RazaoSocial { get; set; }
        public string? Fantasia { get; set; }
        public string CpfCnpj { get; set; } = "";
        public string? Contato { get; set; }
        public string? Municipio { get; set; }
        public string? UF { get; set; }
    }
}
