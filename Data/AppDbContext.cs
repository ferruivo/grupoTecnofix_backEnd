using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.OUT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GrupoTecnofix_Api.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<Empresa> Empresas { get; set; }
    public virtual DbSet<Municipio> Municipios { get; set; }
    public virtual DbSet<Perfi> Perfis { get; set; }
    public virtual DbSet<PerfisPermisso> PerfisPermissoes { get; set; }
    public virtual DbSet<Permisso> Permissoes { get; set; }
    public virtual DbSet<Tipodocumento> Tipodocumentos { get; set; }
    public virtual DbSet<TokensAtualizacao> TokensAtualizacaos { get; set; }
    public virtual DbSet<Transportadora> Transportadoras { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<UsuariosPerfi> UsuariosPerfis { get; set; }
    public virtual DbSet<UsuariosPermisso> UsuariosPermissoes { get; set; }
    public virtual DbSet<Vendedore> Vendedores { get; set; }
    public virtual DbSet<OrigensCadastro> OrigensCadastros { get; set; }
    public virtual DbSet<Fornecedore> Fornecedores { get; set; }
    public virtual DbSet<Condicoespagamento> Condicoespagamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("CLIENTES_PK");

            entity.HasIndex(e => e.Cnpj, "CLIENTES__UQ_CNPJ")
                .IsUnique()
                .HasFilter("([CNPJ] IS NOT NULL)");

            entity.HasIndex(e => e.Cpf, "CLIENTES__UQ_CPF")
                .IsUnique()
                .HasFilter("([CPF] IS NOT NULL)");

            entity.Property(e => e.IpiBc).HasDefaultValue(false);
            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("EMPRESA_PK");

            entity.Property(e => e.Regime).IsFixedLength();
        });

        modelBuilder.Entity<Municipio>(entity =>
        {
            entity.HasKey(e => e.IdMunicipio).HasName("MUNICIPIOS_PK");
        });

        modelBuilder.Entity<Perfi>(entity =>
        {
            entity.HasKey(e => e.IdPerfil).HasName("PERFIS_PK");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<PerfisPermisso>(entity =>
        {
            entity.HasKey(e => new { e.IdPerfil, e.IdPermissao }).HasName("PERFIS_PERMISSOES_PK");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Permisso>(entity =>
        {
            entity.HasKey(e => e.IdPermissao).HasName("PERMISSOES_PK");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Tipodocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipodocumento).HasName("TIPODOCUMENTO_PK");
        });

        modelBuilder.Entity<TokensAtualizacao>(entity =>
        {
            entity.HasKey(e => e.IdTokenAtualizacao).HasName("TOKENS_ATUALIZACAO_PK");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Transportadora>(entity =>
        {
            entity.HasKey(e => e.IdTransportadora).HasName("TRANSPORTADORAS_PK");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("USUARIOS_PK");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<UsuariosPerfi>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdPerfil }).HasName("USUARIOS_PERFIS_PK");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<UsuariosPermisso>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdPermissao }).HasName("USUARIOS_PERMISSOES_PK");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Vendedore>(entity =>
        {
            entity.HasKey(e => e.IdVendedor).HasName("VENDEDORES_PK");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<OrigensCadastro>(entity =>
        {
            entity.HasKey(e => e.IdOrigem).HasName("ORIGENS_CADASTRO_PK");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Fornecedore>(entity =>
        {
            entity.Property(e => e.Icms).HasDefaultValue("N");
            entity.Property(e => e.Ipi).HasDefaultValue("N");
        });

        modelBuilder.Entity<Condicoespagamento>(entity =>
        {
            entity.Property(e => e.Forames).HasDefaultValue("N");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
