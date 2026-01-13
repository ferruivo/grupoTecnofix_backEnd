using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("PERFIS")]
[Index("Nome", Name = "PERFIS__UN", IsUnique = true)]
public partial class Perfi
{
    [Key]
    [Column("ID_PERFIL")]
    public int IdPerfil { get; set; }

    [Column("NOME")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column("DESCRICAO")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Descricao { get; set; }

    [Column("ATIVO")]
    public bool Ativo { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }

    //[InverseProperty("IdPerfilNavigation")]
    //public virtual ICollection<PerfisPermisso> PerfisPermissos { get; set; } = new List<PerfisPermisso>();

    //[InverseProperty("IdPerfilNavigation")]
    //public virtual ICollection<UsuariosPerfi> UsuariosPerfis { get; set; } = new List<UsuariosPerfi>();
}
