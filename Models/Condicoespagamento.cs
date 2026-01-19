using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Condicoespagamento
{
    public int IdCondicoespagamento { get; set; }

    public string Descricao { get; set; } = null!;

    public string Venc01 { get; set; } = null!;

    public string? Venc02 { get; set; }

    public string? Venc03 { get; set; }

    public string? Venc04 { get; set; }

    public string? Venc05 { get; set; }

    public string? Venc06 { get; set; }

    public string? Forames { get; set; }

    public DateTime? DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    
}
