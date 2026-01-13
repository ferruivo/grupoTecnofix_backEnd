using System;
using System.Collections.Generic;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

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

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");

            //entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.ClienteIdMunicipioNavigations)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("CLIENTES_MUNICIPIO_FK");

            //entity.HasOne(d => d.IdMunicipioCobrancaNavigation).WithMany(p => p.ClienteIdMunicipioCobrancaNavigations).HasConstraintName("CLIENTES_MUNICIPIO_COBRANCA_FK");

            //entity.HasOne(d => d.IdTipodocumentoNavigation).WithMany(p => p.Clientes)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("CLIENTES_TIPODOCUMENTO_FK");

            //entity.HasOne(d => d.IdTransportadoraNavigation).WithMany(p => p.Clientes)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("CLIENTES_TRANSPORTADORAS_FK");

            //entity.HasOne(d => d.IdUsuarioAlteracaoNavigation).WithMany(p => p.ClienteIdUsuarioAlteracaoNavigations).HasConstraintName("CLIENTES_USUARIOS_FK_ALT");

            //entity.HasOne(d => d.IdUsuarioCadastroNavigation).WithMany(p => p.ClienteIdUsuarioCadastroNavigations).HasConstraintName("CLIENTES_USUARIOS_FK_CAD");

            //entity.HasOne(d => d.IdVendedorexternoNavigation).WithMany(p => p.ClienteIdVendedorexternoNavigations)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("CLIENTES_VEND_EXT_FK");

            //entity.HasOne(d => d.IdVendedorinternoNavigation).WithMany(p => p.ClienteIdVendedorinternoNavigations)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("CLIENTES_VEND_INT_FK");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("EMPRESA_PK");

            entity.Property(e => e.Regime).IsFixedLength();

            //entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.Empresas)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("EMPRESA_MUNICIPIOS_FK");
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

            //entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.PerfisPermissos)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("PERFIS_PERMISSOES_PERFIS_FK");

            //entity.HasOne(d => d.IdPermissaoNavigation).WithMany(p => p.PerfisPermissos)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("PERFIS_PERMISSOES_PERMISSOES_FK");
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

            //entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TokensAtualizacaos)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("TOKENS_ATUALIZACAO_USUARIOS_FK");
        });

        modelBuilder.Entity<Transportadora>(entity =>
        {
            entity.HasKey(e => e.IdTransportadora).HasName("TRANSPORTADORAS_PK");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");

            //entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.Transportadoras)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("TRANSPORTADORAS_MUNICIPIOS_FK");

            //entity.HasOne(d => d.IdUsuarioAlteracaoNavigation).WithMany(p => p.TransportadoraIdUsuarioAlteracaoNavigations).HasConstraintName("TRANSPORTADORAS_USUARIOS_FK_ALT");

            //entity.HasOne(d => d.IdUsuarioCadastroNavigation).WithMany(p => p.TransportadoraIdUsuarioCadastroNavigations).HasConstraintName("TRANSPORTADORAS_USUARIOS_FK_CAD");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("USUARIOS_PK");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");

            //entity.HasOne(d => d.IdUsuarioAlteracaoNavigation).WithMany(p => p.InverseIdUsuarioAlteracaoNavigation).HasConstraintName("USUARIOS_USUARIOS_FK_ALT");

            //entity.HasOne(d => d.IdUsuarioCadastroNavigation).WithMany(p => p.InverseIdUsuarioCadastroNavigation).HasConstraintName("USUARIOS_USUARIOS_FK_CAD");
        });

        modelBuilder.Entity<UsuariosPerfi>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdPerfil }).HasName("USUARIOS_PERFIS_PK");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");

            //entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.UsuariosPerfis)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("USUARIOS_PERFIS_PERFIS_FK");

            //entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuariosPerfis)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("USUARIOS_PERFIS_USUARIOS_FK");
        });

        modelBuilder.Entity<UsuariosPermisso>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdPermissao }).HasName("USUARIOS_PERMISSOES_PK");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");

            //entity.HasOne(d => d.IdPermissaoNavigation).WithMany(p => p.UsuariosPermissos)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("USUARIOS_PERMISSOES_PERMISSOES_FK");

            //entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuariosPermissos)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("USUARIOS_PERMISSOES_USUARIOS_FK");
        });

        modelBuilder.Entity<Vendedore>(entity =>
        {
            entity.HasKey(e => e.IdVendedor).HasName("VENDEDORES_PK");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(sysutcdatetime())");

            //entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.VendedoreIdUsuarioNavigation)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("VENDEDORES_USUARIOS_FK");

            //entity.HasOne(d => d.IdUsuarioAlteracaoNavigation).WithMany(p => p.VendedoreIdUsuarioAlteracaoNavigations).HasConstraintName("VENDEDORES_USUARIOS_FK_ALT");

            //entity.HasOne(d => d.IdUsuarioCadastroNavigation).WithMany(p => p.VendedoreIdUsuarioCadastroNavigations).HasConstraintName("VENDEDORES_USUARIOS_FK_CAD");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
