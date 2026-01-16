using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Transportadoras;

namespace GrupoTecnofix_Api.Dtos.Fornecedor
{
    public class FornecedorListDto
    {
        public int IdFornecedor { get; set; }
        public string? RazaoSocial { get; set; }
        public string? Fantasia { get; set; }
        public string CpfCnpj { get; set; } = "";
        public string? Contato { get; set; }
    }
}
