using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class OrigensCadastro
{
    public int IdOrigem { get; set; }

    public string Descricao { get; set; } = null!;

    public bool Ativo { get; set; }

    public DateTime DataCadastro { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
