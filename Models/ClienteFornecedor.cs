using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrupoTecnofix_Api.Models;

public partial class ClienteFornecedor
{
    public int IdCliente { get; set; }

    public int IdFornecedor { get; set; }

    public byte Tipo { get; set; }

    public string? Obs { get; set; }

    public DateTime? DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    [NotMapped]
    public string? FornecedorNome { get; set; }
}
