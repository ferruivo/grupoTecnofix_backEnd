using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Models;

[Table("MUNICIPIOS")]
[Index("Nome", Name = "MUNICIPIOS__IDX")]
[Index("Nome", "Uf", Name = "MUNICIPIOS__UN", IsUnique = true)]
public partial class Municipio
{
    [Key]
    [Column("ID_MUNICIPIO")]
    public int IdMunicipio { get; set; }

    [Column("NOME")]
    [StringLength(150)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column("UF")]
    [StringLength(2)]
    [Unicode(false)]
    public string Uf { get; set; } = null!;

    [Column("COD_IBGE")]
    public int? CodIbge { get; set; }

    [Column("COD_IBGE_UF")]
    public int? CodIbgeUf { get; set; }

    //[InverseProperty("IdMunicipioCobrancaNavigation")]
    //public virtual ICollection<Cliente> ClienteIdMunicipioCobrancaNavigations { get; set; } = new List<Cliente>();

    //[InverseProperty("IdMunicipioNavigation")]
    //public virtual ICollection<Cliente> ClienteIdMunicipioNavigations { get; set; } = new List<Cliente>();

    //[InverseProperty("IdMunicipioNavigation")]
    //public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();

    //[InverseProperty("IdMunicipioNavigation")]
    //public virtual ICollection<Transportadora> Transportadoras { get; set; } = new List<Transportadora>();
}
