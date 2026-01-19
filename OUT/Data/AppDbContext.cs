using System;
using System.Collections.Generic;
using GrupoTecnofix_Api.OUT.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.OUT.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Precocompra> Precocompras { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Precocompra>(entity =>
        {
            entity.HasKey(e => e.IdPrecocompra);

            entity.ToTable("PRECOCOMPRA");

            entity.HasIndex(e => new { e.IdProduto, e.IdFornecedor }, "UQ_PRECOCOMPRA_ID_PRODUTO_ID_FORNECEDOR").IsUnique();

            entity.Property(e => e.IdPrecocompra).HasColumnName("ID_PRECOCOMPRA");
            entity.Property(e => e.DataAlteracao)
                .HasPrecision(0)
                .HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.IdFornecedor).HasColumnName("ID_FORNECEDOR");
            entity.Property(e => e.IdProduto).HasColumnName("ID_PRODUTO");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("ID_USUARIO_CADASTRO");
            entity.Property(e => e.Obs)
                .IsUnicode(false)
                .HasColumnName("OBS");
            entity.Property(e => e.Preco)
                .HasColumnType("decimal(16, 5)")
                .HasColumnName("PRECO");
            entity.Property(e => e.Precoantigo)
                .HasColumnType("decimal(16, 5)")
                .HasColumnName("PRECOANTIGO");
            entity.Property(e => e.Vigencia)
                .HasPrecision(0)
                .HasColumnName("VIGENCIA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
