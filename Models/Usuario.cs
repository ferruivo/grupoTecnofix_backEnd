using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("USUARIOS")]
[Index("Login", Name = "USUARIOS__IDX")]
[Index("Email", "Login", Name = "USUARIOS__UN", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [Column("NOME_COMPLETO")]
    [StringLength(150)]
    [Unicode(false)]
    public string NomeCompleto { get; set; } = null!;

    [Column("NOME_EXIBICAO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NomeExibicao { get; set; }

    [Column("EMAIL")]
    [StringLength(150)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("LOGIN")]
    [StringLength(50)]
    [Unicode(false)]
    public string Login { get; set; } = null!;

    [Column("SENHA_HASH")]
    [StringLength(255)]
    [Unicode(false)]
    public string SenhaHash { get; set; } = null!;

    [Column("ATIVO")]
    public bool Ativo { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }

    [Column("ID_USUARIO_CADASTRO")]
    public int? IdUsuarioCadastro { get; set; }

    [Column("DATA_ALTERACAO")]
    public DateTime? DataAlteracao { get; set; }

    [Column("ID_USUARIO_ALTERACAO")]
    public int? IdUsuarioAlteracao { get; set; }

    //[InverseProperty("IdUsuarioAlteracaoNavigation")]
    //public virtual ICollection<Cliente> ClienteIdUsuarioAlteracaoNavigations { get; set; } = new List<Cliente>();

    //[InverseProperty("IdUsuarioCadastroNavigation")]
    //public virtual ICollection<Cliente> ClienteIdUsuarioCadastroNavigations { get; set; } = new List<Cliente>();

    //[ForeignKey("IdUsuarioAlteracao")]
    //[InverseProperty("InverseIdUsuarioAlteracaoNavigation")]
    //public virtual Usuario? IdUsuarioAlteracaoNavigation { get; set; }

    //[ForeignKey("IdUsuarioCadastro")]
    //[InverseProperty("InverseIdUsuarioCadastroNavigation")]
    //public virtual Usuario? IdUsuarioCadastroNavigation { get; set; }

    [NotMapped]
    public UsuariosPerfi UsuariosPerfil { get; set; }

    //[InverseProperty("IdUsuarioAlteracaoNavigation")]
    //public virtual ICollection<Usuario> InverseIdUsuarioAlteracaoNavigation { get; set; } = new List<Usuario>();

    //[InverseProperty("IdUsuarioCadastroNavigation")]
    //public virtual ICollection<Usuario> InverseIdUsuarioCadastroNavigation { get; set; } = new List<Usuario>();

    //[InverseProperty("IdUsuarioNavigation")]
    //public virtual ICollection<TokensAtualizacao> TokensAtualizacaos { get; set; } = new List<TokensAtualizacao>();

    //[InverseProperty("IdUsuarioAlteracaoNavigation")]
    //public virtual ICollection<Transportadora> TransportadoraIdUsuarioAlteracaoNavigations { get; set; } = new List<Transportadora>();

    //[InverseProperty("IdUsuarioCadastroNavigation")]
    //public virtual ICollection<Transportadora> TransportadoraIdUsuarioCadastroNavigations { get; set; } = new List<Transportadora>();

    //[InverseProperty("IdUsuarioNavigation")]
    //public virtual ICollection<UsuariosPerfi> UsuariosPerfis { get; set; } = new List<UsuariosPerfi>();

    //[InverseProperty("IdUsuarioNavigation")]
    //public virtual ICollection<UsuariosPermisso> UsuariosPermissos { get; set; } = new List<UsuariosPermisso>();

    //[InverseProperty("IdUsuarioAlteracaoNavigation")]
    //public virtual ICollection<Vendedore> VendedoreIdUsuarioAlteracaoNavigations { get; set; } = new List<Vendedore>();

    //[InverseProperty("IdUsuarioCadastroNavigation")]
    //public virtual ICollection<Vendedore> VendedoreIdUsuarioCadastroNavigations { get; set; } = new List<Vendedore>();

    //[InverseProperty("IdUsuarioNavigation")]
    //public virtual Vendedore? VendedoreIdUsuarioNavigation { get; set; }
}
