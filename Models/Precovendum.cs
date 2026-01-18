using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Precovendum
{
    public int IdPrecovenda { get; set; }

    public int IdProduto { get; set; }

    public int IdCliente { get; set; }

    public decimal Preco { get; set; }

    public DateTime? Vigencia { get; set; }

    public string? Revisao { get; set; }

    public DateTime? Datarevisao { get; set; }

    public string? Usurevisao { get; set; }

    public decimal? Precoantigo { get; set; }

    public string? Obs { get; set; }

    public string? IdUsuarioCadastro { get; set; }

    public DateTime? DataCadastro { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Produto IdProdutoNavigation { get; set; } = null!;
}
