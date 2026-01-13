using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("EMPRESA")]
[Index("Cnpj", Name = "EMPRESA__UN", IsUnique = true)]
public partial class Empresa
{
    [Key]
    [Column("ID_EMPRESA")]
    public int IdEmpresa { get; set; }

    [Column("RAZAO_SOCIAL")]
    [StringLength(150)]
    [Unicode(false)]
    public string RazaoSocial { get; set; } = null!;

    [Column("NOME_FANTASIA")]
    [StringLength(50)]
    [Unicode(false)]
    public string NomeFantasia { get; set; } = null!;

    [Column("CNPJ")]
    [StringLength(14)]
    [Unicode(false)]
    public string Cnpj { get; set; } = null!;

    [Column("INSCRICAO_ESTADUAL")]
    [StringLength(50)]
    [Unicode(false)]
    public string? InscricaoEstadual { get; set; }

    [Column("ENDERECO")]
    [StringLength(150)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [Column("BAIRRO")]
    [StringLength(150)]
    [Unicode(false)]
    public string Bairro { get; set; } = null!;

    [Column("CEP")]
    [StringLength(8)]
    [Unicode(false)]
    public string Cep { get; set; } = null!;

    [Column("NUMERO")]
    public int Numero { get; set; }

    [Column("COMPLEMENTO")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Complemento { get; set; }

    [Column("ID_MUNICIPIO")]
    public int IdMunicipio { get; set; }

    [Column("TELEFONE")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Telefone { get; set; }

    [Column("REGIME")]
    [StringLength(1)]
    [Unicode(false)]
    public string Regime { get; set; } = null!;

    [Column("ALIQUOTA_REC_ICMS", TypeName = "decimal(16, 2)")]
    public decimal? AliquotaRecIcms { get; set; }

    //[ForeignKey("IdMunicipio")]
    //[InverseProperty("Empresas")]
    //public virtual Municipio IdMunicipioNavigation { get; set; } = null!;
}
