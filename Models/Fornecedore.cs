using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Fornecedore
{
    public int IdFornecedor { get; set; }

    public string? Razaosocial { get; set; }

    public string? Fantasia { get; set; }

    public string Cpfcnpj { get; set; } = null!;

    public string Ie { get; set; } = null!;

    public string? Contato { get; set; }

    public string? Telefone { get; set; }

    public string? Email { get; set; }

    public string Cep { get; set; } = null!;

    public string Endereco { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public string Bairro { get; set; } = null!;

    public string? Complemento { get; set; }

    public int IdMunicipio { get; set; }

    public int? IdPagamento { get; set; }

    public int? IdTransportadora { get; set; }

    public string Ipi { get; set; } = null!;

    public string Icms { get; set; } = null!;

    public string? Frete { get; set; }

    public string? Obs { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataCadastro { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public DateTime? DataAlteracao { get; set; }

   
}
