using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Municipio
{
    public int IdMunicipio { get; set; }

    public string Nome { get; set; } = null!;

    public string Uf { get; set; } = null!;

    public int? CodIbge { get; set; }

    public int? CodIbgeUf { get; set; }

    
}
