using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class ProdutoKitIten
{
    public int IdProdutoKit { get; set; }

    public int IdProduto { get; set; }

    public int Quantidade { get; set; }

    public DateTime? DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }
}
