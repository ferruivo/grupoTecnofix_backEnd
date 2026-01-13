using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[PrimaryKey("IdUsuario", "IdPerfil")]
[Table("USUARIOS_PERFIS")]
public partial class UsuariosPerfi
{
    [Key]
    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [Key]
    [Column("ID_PERFIL")]
    public int IdPerfil { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }

    //[ForeignKey("IdPerfil")]
    //[InverseProperty("UsuariosPerfis")]
    //public virtual Perfi IdPerfilNavigation { get; set; } = null!;

    //[ForeignKey("IdUsuario")]
    //[InverseProperty("UsuariosPerfis")]
    //public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
