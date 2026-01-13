namespace GrupoTecnofix_Api.Dtos.Cliente
{
    public class ClienteCreateUpdate
    {
        public int IdTipoDocumento { get; set; }
        public string? Cpf { get; set; }
        public string? Cnpj { get; set; }
        public string? InscricaoEstadual { get; set; }

        public string Nome { get; set; } = null!;
        public string Fantasia { get; set; } = null!;
        public string Contato { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string Cep { get; set; } = null!;
        public string Endereco { get; set; } = null!;
        public string Bairro { get; set; } = null!;
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public int IdMunicipio { get; set; }

        public string? CepCobranca { get; set; }
        public string? EnderecoCobranca { get; set; }
        public string? BairroCobranca { get; set; }
        public int? NumeroCobranca { get; set; }
        public string? ComplementoCobranca { get; set; }
        public int? IdMunicipioCobranca { get; set; }

        public int IdVendedorInterno { get; set; }
        public int IdVendedorExterno { get; set; }
        public int IdTransportadora { get; set; }
    }
}
