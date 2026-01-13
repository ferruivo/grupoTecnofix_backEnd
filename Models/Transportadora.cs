using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("TRANSPORTADORAS")]
[Index("Cnpj", Name = "TRANSPORTADORAS__UN", IsUnique = true)]
public partial class Transportadora
{
    [Key]
    [Column("ID_TRANSPORTADORA")]
    public int IdTransportadora { get; set; }

    [Column("CNPJ")]
    [StringLength(14)]
    [Unicode(false)]
    public string Cnpj { get; set; } = null!;

    [Column("INSCRICAO_ESTADUAL")]
    [StringLength(50)]
    [Unicode(false)]
    public string? InscricaoEstadual { get; set; }

    [Column("RAZAO_SOCIAL")]
    [StringLength(150)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;

    [Column("FANTASIA")]
    [StringLength(50)]
    [Unicode(false)]
    public string Fantasia { get; set; } = null!;

    [Column("CONTATO")]
    [StringLength(50)]
    [Unicode(false)]
    public string Contato { get; set; } = null!;

    [Column("TELEFONE")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Telefone { get; set; }

    [Column("CEP")]
    [StringLength(8)]
    [Unicode(false)]
    public string Cep { get; set; } = null!;

    [Column("ENDERECO")]
    [StringLength(150)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [Column("BAIRRO")]
    [StringLength(50)]
    [Unicode(false)]
    public string Bairro { get; set; } = null!;

    [Column("NUMERO")]
    public int Numero { get; set; }

    [Column("COMPLEMENTO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Complemento { get; set; }

    [Column("ID_MUNICIPIO")]
    public int IdMunicipio { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; }

    [Column("ID_USUARIO_CADASTRO")]
    public int? IdUsuarioCadastro { get; set; }

    [Column("DATA_ALTERACAO")]
    public DateTime? DataAlteracao { get; set; }

    [Column("ID_USUARIO_ALTERACAO")]
    public int? IdUsuarioAlteracao { get; set; }

    //[InverseProperty("IdTransportadoraNavigation")]
    //public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    //[ForeignKey("IdMunicipio")]
    //[InverseProperty("Transportadoras")]
    //public virtual Municipio IdMunicipioNavigation { get; set; } = null!;

    //[ForeignKey("IdUsuarioAlteracao")]
    //[InverseProperty("TransportadoraIdUsuarioAlteracaoNavigations")]
    //public virtual Usuario? IdUsuarioAlteracaoNavigation { get; set; }

    //[ForeignKey("IdUsuarioCadastro")]
    //[InverseProperty("TransportadoraIdUsuarioCadastroNavigations")]
    //public virtual Usuario? IdUsuarioCadastroNavigation { get; set; }
}
