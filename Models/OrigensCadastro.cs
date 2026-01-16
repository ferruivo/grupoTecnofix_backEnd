using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.OUT.Models;

[Table("ORIGENS_CADASTRO")]
public partial class OrigensCadastro
{
    [Key]
    [Column("ID_ORIGEM")]
    public int IdOrigem { get; set; }

    [Column("DESCRICAO")]
    [StringLength(80)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("ATIVO")]
    public bool Ativo { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }
}
