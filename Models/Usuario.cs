using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NomeCompleto { get; set; } = null!;

    public string? NomeExibicao { get; set; }

    public string Email { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string SenhaHash { get; set; } = null!;

    public bool Ativo { get; set; }

    public DateTime DataCadastro { get; set; }

    public int? IdUsuarioCadastro { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public int? IdUsuarioAlteracao { get; set; }

    public virtual ICollection<Cliente> ClienteIdUsuarioAlteracaoNavigations { get; set; } = new List<Cliente>();

    public virtual ICollection<Cliente> ClienteIdUsuarioCadastroNavigations { get; set; } = new List<Cliente>();

    public virtual Usuario? IdUsuarioAlteracaoNavigation { get; set; }

    public virtual Usuario? IdUsuarioCadastroNavigation { get; set; }

    public virtual ICollection<Usuario> InverseIdUsuarioAlteracaoNavigation { get; set; } = new List<Usuario>();

    public virtual ICollection<Usuario> InverseIdUsuarioCadastroNavigation { get; set; } = new List<Usuario>();

    public virtual ICollection<Produto> ProdutoIdUsuarioAlteracaoNavigations { get; set; } = new List<Produto>();

    public virtual ICollection<Produto> ProdutoIdUsuarioCadastroNavigations { get; set; } = new List<Produto>();

    public virtual ICollection<TokensAtualizacao> TokensAtualizacaos { get; set; } = new List<TokensAtualizacao>();

    public virtual ICollection<Transportadora> TransportadoraIdUsuarioAlteracaoNavigations { get; set; } = new List<Transportadora>();

    public virtual ICollection<Transportadora> TransportadoraIdUsuarioCadastroNavigations { get; set; } = new List<Transportadora>();

    public virtual ICollection<UsuariosPerfi> UsuariosPerfis { get; set; } = new List<UsuariosPerfi>();

    public virtual ICollection<Vendedore> VendedoreIdUsuarioAlteracaoNavigations { get; set; } = new List<Vendedore>();

    public virtual ICollection<Vendedore> VendedoreIdUsuarioCadastroNavigations { get; set; } = new List<Vendedore>();

    public virtual Vendedore? VendedoreIdUsuarioNavigation { get; set; }
}
