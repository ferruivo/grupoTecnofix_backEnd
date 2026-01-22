using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Prateleira
{
    public int? IdPrateleira { get; set; }

    public string Descricao { get; set; } = null!;

    public DateTime? DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public string? DescricaoNorm { get; set; }
}
