using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[PrimaryKey("IdPerfil", "IdPermissao")]
[Table("PERFIS_PERMISSOES")]
public partial class PerfisPermisso
{
    [Key]
    [Column("ID_PERFIL")]
    public int IdPerfil { get; set; }

    [Key]
    [Column("ID_PERMISSAO")]
    public int IdPermissao { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }

    //[ForeignKey("IdPerfil")]
    //[InverseProperty("PerfisPermissos")]
    //public virtual Perfi IdPerfilNavigation { get; set; } = null!;

    //[ForeignKey("IdPermissao")]
    //[InverseProperty("PerfisPermissos")]
    //public virtual Permisso IdPermissaoNavigation { get; set; } = null!;
}
