using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Transportadoras;

namespace GrupoTecnofix_Api.Dtos.Fornecedor
{
    public class FornecedorDto
    {
        public int IdFornecedor { get; set; }

        public string? RazaoSocial { get; set; }
        public string? Fantasia { get; set; }

        public string CpfCnpj { get; set; } = "";
        public string Ie { get; set; } = "";

        public string? Contato { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }

        public string Cep { get; set; } = "";
        public string Endereco { get; set; } = "";
        public string Numero { get; set; } = "";
        public string Bairro { get; set; } = "";
        public string? Complemento { get; set; }

        public int IdMunicipio { get; set; }
        public int? IdPagamento { get; set; }
        public int? IdTransportadora { get; set; }

        public string Ipi { get; set; } = "N";   // 'N'/'S'
        public string Icms { get; set; } = "N";  // 'N'/'S'

        public string? Frete { get; set; }
        public string? Obs { get; set; }

        public string? UsuarioCadastro { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string? UsuarioAlteracao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        // objetos para edição (preencher combos)
        public MunicipioDto? Municipio { get; set; }
        public CondicaoPagamentoDto? CondicaoPagamento { get; set; }
        public TransportadoraListDto? Transportadora { get; set; }
    }
}
