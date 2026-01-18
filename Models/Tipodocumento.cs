using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Tipodocumento
{
    public int IdTipodocumento { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
