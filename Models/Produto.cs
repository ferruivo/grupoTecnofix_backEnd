using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Produto
{
    public int IdProduto { get; set; }

    public string Codigo { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public string Ncm { get; set; } = null!;

    public string Unidade { get; set; } = null!;

    public string CstIpi { get; set; } = null!;

    public string CstIcms { get; set; } = null!;

    public int? Minimo { get; set; }

    public string? Obs { get; set; }

    public string? ObsNf { get; set; }

    public string? ObsEntrada { get; set; }

    public DateOnly? DataInclusao { get; set; }

    public int FatorEmbalagem { get; set; }

    public DateTime? DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public virtual Usuario? IdUsuarioAlteracaoNavigation { get; set; }

    public virtual Usuario? IdUsuarioCadastroNavigation { get; set; }

    public virtual ICollection<Precovendum> Precovenda { get; set; } = new List<Precovendum>();
}
