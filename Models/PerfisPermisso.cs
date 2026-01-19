using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class PerfisPermisso
{
    public int IdPerfil { get; set; }

    public int IdPermissao { get; set; }

    public DateTime DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    
}
