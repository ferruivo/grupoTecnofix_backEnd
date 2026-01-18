using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Vendedore
{
    public int IdVendedor { get; set; }

    public int IdUsuario { get; set; }

    public bool Interno { get; set; }

    public bool Externo { get; set; }

    public string? Observacao { get; set; }

    public DateTime DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public virtual ICollection<Cliente> ClienteIdVendedorexternoNavigations { get; set; } = new List<Cliente>();

    public virtual ICollection<Cliente> ClienteIdVendedorinternoNavigations { get; set; } = new List<Cliente>();

    public virtual Usuario? IdUsuarioAlteracaoNavigation { get; set; }

    public virtual Usuario? IdUsuarioCadastroNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
