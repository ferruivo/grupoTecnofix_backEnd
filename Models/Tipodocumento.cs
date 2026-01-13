using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("TIPODOCUMENTO")]
[Index("Descricao", Name = "TIPODOCUMENTO__UN", IsUnique = true)]
public partial class Tipodocumento
{
    [Key]
    [Column("ID_TIPODOCUMENTO")]
    public int IdTipodocumento { get; set; }

    [Column("DESCRICAO")]
    [StringLength(5)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    //[InverseProperty("IdTipodocumentoNavigation")]
    //public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
