using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public int IdTipodocumento { get; set; }

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

    public int IdVendedorinterno { get; set; }

    public int IdVendedorexterno { get; set; }

    public int IdTransportadora { get; set; }

    public DateTime? DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public string? Suframa { get; set; }

    public string? Observacao { get; set; }

    public string? ObservacaoOrdemExpedicao { get; set; }

    public string? ObservacaoNotaFiscal { get; set; }

    public int IdOrigem { get; set; }

    public bool IpiBc { get; set; }
}
