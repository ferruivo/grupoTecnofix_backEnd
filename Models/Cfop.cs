using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Cfop
{
    public int IdCfop { get; set; }

    public string Cfop1 { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public string TipoOperacao { get; set; } = null!;

    public string? Venda { get; set; }

    public string? Icms { get; set; }

    public string? Obs { get; set; }

    public string? Devolucao { get; set; }

    public string? Interestadual { get; set; }

    public DateTime DataCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }
}
