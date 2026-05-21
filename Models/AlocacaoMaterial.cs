using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class AlocacaoMaterial
{
    public int IdAlocacaoMaterial { get; set; }

    public int IdLote { get; set; }

    public int Etq { get; set; }

    public string IdPrateleira { get; set; } = null!;

    public int Qtd { get; set; }

    public int Reservado { get; set; }

    public DateTime? DataAlocacao { get; set; }
}
