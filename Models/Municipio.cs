using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Municipio
{
    public int IdMunicipio { get; set; }

    public string Nome { get; set; } = null!;

    public string Uf { get; set; } = null!;

    public int? CodIbge { get; set; }

    public int? CodIbgeUf { get; set; }

    public virtual ICollection<Cliente> ClienteIdMunicipioCobrancaNavigations { get; set; } = new List<Cliente>();

    public virtual ICollection<Cliente> ClienteIdMunicipioNavigations { get; set; } = new List<Cliente>();

    public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();

    public virtual ICollection<Fornecedore> Fornecedores { get; set; } = new List<Fornecedore>();

    public virtual ICollection<Transportadora> Transportadoras { get; set; } = new List<Transportadora>();
}
