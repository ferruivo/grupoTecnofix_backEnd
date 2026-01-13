using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[PrimaryKey("IdUsuario", "IdPermissao")]
[Table("USUARIOS_PERMISSOES")]
public partial class UsuariosPermisso
{
    [Key]
    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [Key]
    [Column("ID_PERMISSAO")]
    public int IdPermissao { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }

    //[ForeignKey("IdPermissao")]
    //[InverseProperty("UsuariosPermissos")]
    //public virtual Permisso IdPermissaoNavigation { get; set; } = null!;

    //[ForeignKey("IdUsuario")]
    //[InverseProperty("UsuariosPermissos")]
    //public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
