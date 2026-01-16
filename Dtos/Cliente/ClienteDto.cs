using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Dtos.Vendedor;

namespace GrupoTecnofix_Api.Dtos.Cliente
{
    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public int IdTipodocumento { get; set; }
        public string? Cpf { get; set; }
        public string? Cnpj { get; set; }
        public string? InscricaoEstadual { get; set; }

        public string Nome { get; set; } = "";
        public string Fantasia { get; set; } = "";
        public string Contato { get; set; } = "";
        public string Email { get; set; } = "";

        public string Cep { get; set; } = "";
        public string Endereco { get; set; } = "";
        public string Bairro { get; set; } = "";
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public int IdMunicipio { get; set; }

        public string? CepCobranca { get; set; }
        public string? EnderecoCobranca { get; set; }
        public string? BairroCobranca { get; set; }
        public int? NumeroCobranca { get; set; }
        public string? ComplementoCobranca { get; set; }
        public int? IdMunicipioCobranca { get; set; }

        public int IdVendedorinterno { get; set; }
        public int IdVendedorexterno { get; set; }
        public int IdTransportadora { get; set; }

        public string? Suframa { get; set; }
        public bool IpiBc { get; set; }              // default false
        public int IdOrigem { get; set; }            // FK obrigatório

        public string? Observacao { get; set; }
        public string? ObservacaoOrdemExpedicao { get; set; }
        public string? ObservacaoNotaFiscal { get; set; }

        public MunicipioDto? Municipio { get; set; }
        public VendedorDto? VendedorInterno { get; set; }
        public VendedorDto? VendedorExterno { get; set; }
        public TipoDocumentoDto TipoDocumento { get; set; }
        public OrigemCadastroDto? OrigemCadastro { get; set; }
    }
}
