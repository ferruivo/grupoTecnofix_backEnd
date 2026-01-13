using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("VENDEDORES")]
[Index("IdUsuario", Name = "VENDEDORES__UN", IsUnique = true)]
public partial class Vendedore
{
    [Key]
    [Column("ID_VENDEDOR")]
    public int IdVendedor { get; set; }

    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [Column("INTERNO")]
    public bool Interno { get; set; }

    [Column("EXTERNO")]
    public bool Externo { get; set; }

    [Column("OBSERVACAO")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Observacao { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }

    [Column("ID_USUARIO_CADASTRO")]
    public int? IdUsuarioCadastro { get; set; }

    [Column("DATA_ALTERACAO")]
    public DateTime? DataAlteracao { get; set; }

    [Column("ID_USUARIO_ALTERACAO")]
    public int? IdUsuarioAlteracao { get; set; }

    //[InverseProperty("IdVendedorexternoNavigation")]
    //public virtual ICollection<Cliente> ClienteIdVendedorexternoNavigations { get; set; } = new List<Cliente>();

    //[InverseProperty("IdVendedorinternoNavigation")]
    //public virtual ICollection<Cliente> ClienteIdVendedorinternoNavigations { get; set; } = new List<Cliente>();

    //[ForeignKey("IdUsuarioAlteracao")]
    //[InverseProperty("VendedoreIdUsuarioAlteracaoNavigations")]
    //public virtual Usuario? IdUsuarioAlteracaoNavigation { get; set; }

    //[ForeignKey("IdUsuarioCadastro")]
    //[InverseProperty("VendedoreIdUsuarioCadastroNavigations")]
    //public virtual Usuario? IdUsuarioCadastroNavigation { get; set; }

    //[ForeignKey("IdUsuario")]
    //[InverseProperty("VendedoreIdUsuarioNavigation")]
    //public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
