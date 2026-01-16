using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("CONDICOESPAGAMENTO")]
public partial class Condicoespagamento
{
    [Key]
    [Column("ID_CONDICOESPAGAMENTO")]
    public int IdCondicoespagamento { get; set; }

    [Column("DESCRICAO")]
    [StringLength(45)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    [Column("VENC01")]
    [StringLength(3)]
    [Unicode(false)]
    public string Venc01 { get; set; } = null!;

    [Column("VENC02")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Venc02 { get; set; }

    [Column("VENC03")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Venc03 { get; set; }

    [Column("VENC04")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Venc04 { get; set; }

    [Column("VENC05")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Venc05 { get; set; }

    [Column("VENC06")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Venc06 { get; set; }

    [Column("FORAMES")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Forames { get; set; }
}
