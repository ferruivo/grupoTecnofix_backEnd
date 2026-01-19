using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.OUT.Models;

public partial class Precocompra
{
    public int IdPrecocompra { get; set; }

    public int IdProduto { get; set; }

    public int IdFornecedor { get; set; }

    public decimal Preco { get; set; }

    public DateTime? Vigencia { get; set; }

    public decimal? Precoantigo { get; set; }

    public string? Obs { get; set; }

    public string? IdUsuarioCadastro { get; set; }

    public DateTime? DataCadastro { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public DateTime? DataAlteracao { get; set; }
}
