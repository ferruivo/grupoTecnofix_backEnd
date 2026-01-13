using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("PERMISSOES")]
[Index("Codigo", Name = "PERMISSOES__UN", IsUnique = true)]
public partial class Permisso
{
    [Key]
    [Column("ID_PERMISSAO")]
    public int IdPermissao { get; set; }

    [Column("CODIGO")]
    [StringLength(80)]
    [Unicode(false)]
    public string Codigo { get; set; } = null!;

    [Column("DESCRICAO")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Descricao { get; set; }

    [Column("ATIVO")]
    public bool Ativo { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }

    //[InverseProperty("IdPermissaoNavigation")]
    //public virtual ICollection<PerfisPermisso> PerfisPermissos { get; set; } = new List<PerfisPermisso>();

    //[InverseProperty("IdPermissaoNavigation")]
    //public virtual ICollection<UsuariosPermisso> UsuariosPermissos { get; set; } = new List<UsuariosPermisso>();
}
