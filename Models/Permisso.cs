using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Permisso
{
    public int IdPermissao { get; set; }

    public string Codigo { get; set; } = null!;

    public string? Descricao { get; set; }

    public bool Ativo { get; set; }

    public DateTime DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

   
}
