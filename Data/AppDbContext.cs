using GrupoTecnofix_Api.Models;
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

    public virtual DbSet<Condicoespagamento> Condicoespagamentos { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Fornecedore> Fornecedores { get; set; }

    public virtual DbSet<Municipio> Municipios { get; set; }

    public virtual DbSet<OrigensCadastro> OrigensCadastros { get; set; }

    public virtual DbSet<Perfi> Perfis { get; set; }

    public virtual DbSet<PerfisPermisso> PerfisPermissoes { get; set; }

    public virtual DbSet<Permisso> Permissoes { get; set; }

    public virtual DbSet<Precovendum> Precovenda { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Tipodocumento> Tipodocumentos { get; set; }

    public virtual DbSet<TokensAtualizacao> TokensAtualizacaos { get; set; }

    public virtual DbSet<Transportadora> Transportadoras { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuariosPerfi> UsuariosPerfis { get; set; }

    public virtual DbSet<Vendedore> Vendedores { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("CLIENTES_PK");

            entity.ToTable("CLIENTES");

            entity.HasIndex(e => e.Nome, "CLIENTES__IDX");

            entity.HasIndex(e => e.Cnpj, "CLIENTES__UQ_CNPJ")
                .IsUnique()
                .HasFilter("([CNPJ] IS NOT NULL)");

            entity.HasIndex(e => e.Cpf, "CLIENTES__UQ_CPF")
                .IsUnique()
                .HasFilter("([CPF] IS NOT NULL)");

            entity.HasIndex(e => e.IdOrigem, "IX_CLIENTES_ID_ORIGEM");

            entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BAIRRO");
            entity.Property(e => e.BairroCobranca)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BAIRRO_COBRANCA");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("CEP");
            entity.Property(e => e.CepCobranca)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("CEP_COBRANCA");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("CNPJ");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COMPLEMENTO");
            entity.Property(e => e.ComplementoCobranca)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COMPLEMENTO_COBRANCA");
            entity.Property(e => e.Contato)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTATO");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("CPF");
            entity.Property(e => e.DataAlteracao)
                .HasPrecision(0)
                .HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Endereco)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ENDERECO");
            entity.Property(e => e.EnderecoCobranca)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ENDERECO_COBRANCA");
            entity.Property(e => e.Fantasia)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("FANTASIA");
            entity.Property(e => e.IdMunicipio).HasColumnName("ID_MUNICIPIO");
            entity.Property(e => e.IdMunicipioCobranca).HasColumnName("ID_MUNICIPIO_COBRANCA");
            entity.Property(e => e.IdOrigem).HasColumnName("ID_ORIGEM");
            entity.Property(e => e.IdTipodocumento).HasColumnName("ID_TIPODOCUMENTO");
            entity.Property(e => e.IdTransportadora).HasColumnName("ID_TRANSPORTADORA");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");
            entity.Property(e => e.IdVendedorexterno).HasColumnName("ID_VENDEDOREXTERNO");
            entity.Property(e => e.IdVendedorinterno).HasColumnName("ID_VENDEDORINTERNO");
            entity.Property(e => e.InscricaoEstadual)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INSCRICAO_ESTADUAL");
            entity.Property(e => e.IpiBc).HasColumnName("IPI_BC");
            entity.Property(e => e.Nome)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("NOME");
            entity.Property(e => e.Numero).HasColumnName("NUMERO");
            entity.Property(e => e.NumeroCobranca).HasColumnName("NUMERO_COBRANCA");
            entity.Property(e => e.Observacao)
                .IsUnicode(false)
                .HasColumnName("OBSERVACAO");
            entity.Property(e => e.ObservacaoNotaFiscal)
                .IsUnicode(false)
                .HasColumnName("OBSERVACAO_NOTA_FISCAL");
            entity.Property(e => e.ObservacaoOrdemExpedicao)
                .IsUnicode(false)
                .HasColumnName("OBSERVACAO_ORDEM_EXPEDICAO");
            entity.Property(e => e.Suframa)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SUFRAMA");

            
        });

        modelBuilder.Entity<Condicoespagamento>(entity =>
        {
            entity.HasKey(e => e.IdCondicoespagamento);

            entity.ToTable("CONDICOESPAGAMENTO");

            entity.Property(e => e.IdCondicoespagamento).HasColumnName("ID_CONDICOESPAGAMENTO");
            entity.Property(e => e.DataAlteracao).HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro).HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.Descricao)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO");
            entity.Property(e => e.Forames)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("FORAMES");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");
            entity.Property(e => e.Venc01)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("VENC01");
            entity.Property(e => e.Venc02)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("VENC02");
            entity.Property(e => e.Venc03)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("VENC03");
            entity.Property(e => e.Venc04)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("VENC04");
            entity.Property(e => e.Venc05)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("VENC05");
            entity.Property(e => e.Venc06)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("VENC06");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("EMPRESA_PK");

            entity.ToTable("EMPRESA");

            entity.HasIndex(e => e.Cnpj, "EMPRESA__UN").IsUnique();

            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA");
            entity.Property(e => e.AliquotaRecIcms)
                .HasColumnType("decimal(16, 2)")
                .HasColumnName("ALIQUOTA_REC_ICMS");
            entity.Property(e => e.Bairro)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("BAIRRO");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("CEP");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("CNPJ");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COMPLEMENTO");
            entity.Property(e => e.Endereco)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ENDERECO");
            entity.Property(e => e.IdMunicipio).HasColumnName("ID_MUNICIPIO");
            entity.Property(e => e.InscricaoEstadual)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INSCRICAO_ESTADUAL");
            entity.Property(e => e.NomeFantasia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOME_FANTASIA");
            entity.Property(e => e.Numero).HasColumnName("NUMERO");
            entity.Property(e => e.RazaoSocial)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("RAZAO_SOCIAL");
            entity.Property(e => e.Regime)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGIME");
            entity.Property(e => e.Telefone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TELEFONE");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.Empresas)
                .HasForeignKey(d => d.IdMunicipio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EMPRESA_MUNICIPIOS_FK");
        });

        modelBuilder.Entity<Fornecedore>(entity =>
        {
            entity.HasKey(e => e.IdFornecedor);

            entity.ToTable("FORNECEDORES");

            entity.HasIndex(e => e.IdMunicipio, "IX_FORNECEDORES_ID_MUNICIPIO");

            entity.HasIndex(e => e.IdPagamento, "IX_FORNECEDORES_ID_PAGAMENTO");

            entity.HasIndex(e => e.IdTransportadora, "IX_FORNECEDORES_ID_TRANSPORTADORA");

            entity.Property(e => e.IdFornecedor).HasColumnName("ID_FORNECEDOR");
            entity.Property(e => e.Bairro)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BAIRRO");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("CEP");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COMPLEMENTO");
            entity.Property(e => e.Contato)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTATO");
            entity.Property(e => e.Cpfcnpj)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("CPFCNPJ");
            entity.Property(e => e.DataAlteracao)
                .HasPrecision(0)
                .HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Endereco)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ENDERECO");
            entity.Property(e => e.Fantasia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FANTASIA");
            entity.Property(e => e.Frete)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("FRETE");
            entity.Property(e => e.Icms)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("ICMS");
            entity.Property(e => e.IdMunicipio).HasColumnName("ID_MUNICIPIO");
            entity.Property(e => e.IdPagamento).HasColumnName("ID_PAGAMENTO");
            entity.Property(e => e.IdTransportadora).HasColumnName("ID_TRANSPORTADORA");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");
            entity.Property(e => e.Ie)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IE");
            entity.Property(e => e.Ipi)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("IPI");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("NUMERO");
            entity.Property(e => e.Obs)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("OBS");
            entity.Property(e => e.Razaosocial)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("RAZAOSOCIAL");
            entity.Property(e => e.Telefone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TELEFONE");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.Fornecedores)
                .HasForeignKey(d => d.IdMunicipio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FORNECEDORES_MUNICIPIOS_FK");

            entity.HasOne(d => d.IdPagamentoNavigation).WithMany(p => p.Fornecedores)
                .HasForeignKey(d => d.IdPagamento)
                .HasConstraintName("FORNECEDORES_CONDICOESPAGAMENTO_FK");

            entity.HasOne(d => d.IdTransportadoraNavigation).WithMany(p => p.Fornecedores)
                .HasForeignKey(d => d.IdTransportadora)
                .HasConstraintName("FORNECEDORES_TRANSPORTADORAS_FK");
        });

        modelBuilder.Entity<Municipio>(entity =>
        {
            entity.HasKey(e => e.IdMunicipio).HasName("MUNICIPIOS_PK");

            entity.ToTable("MUNICIPIOS");

            entity.HasIndex(e => e.Nome, "MUNICIPIOS__IDX");

            entity.HasIndex(e => new { e.Nome, e.Uf }, "MUNICIPIOS__UN").IsUnique();

            entity.HasIndex(e => e.CodIbge, "UX_MUNICIPIOS_COD_IBGE").IsUnique();

            entity.Property(e => e.IdMunicipio).HasColumnName("ID_MUNICIPIO");
            entity.Property(e => e.CodIbge).HasColumnName("COD_IBGE");
            entity.Property(e => e.CodIbgeUf).HasColumnName("COD_IBGE_UF");
            entity.Property(e => e.Nome)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("NOME");
            entity.Property(e => e.Uf)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("UF");
        });

        modelBuilder.Entity<OrigensCadastro>(entity =>
        {
            entity.HasKey(e => e.IdOrigem).HasName("ORIGENS_CADASTRO_PK");

            entity.ToTable("ORIGENS_CADASTRO");

            entity.Property(e => e.IdOrigem).HasColumnName("ID_ORIGEM");
            entity.Property(e => e.Ativo)
                .HasDefaultValue(true)
                .HasColumnName("ATIVO");
            entity.Property(e => e.DataCadastro)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.Descricao)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO");
        });

        modelBuilder.Entity<Perfi>(entity =>
        {
            entity.HasKey(e => e.IdPerfil).HasName("PERFIS_PK");

            entity.ToTable("PERFIS");

            entity.HasIndex(e => e.Nome, "PERFIS__UN").IsUnique();

            entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");
            entity.Property(e => e.Ativo)
                .HasDefaultValue(true)
                .HasColumnName("ATIVO");
            entity.Property(e => e.DataAlteracao)
                .HasPrecision(0)
                .HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.Descricao)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOME");
        });

        modelBuilder.Entity<PerfisPermisso>(entity =>
        {
            entity.HasKey(e => new { e.IdPerfil, e.IdPermissao }).HasName("PERFIS_PERMISSOES_PK");

            entity.ToTable("PERFIS_PERMISSOES");

            entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");
            entity.Property(e => e.IdPermissao).HasColumnName("ID_PERMISSAO");
            entity.Property(e => e.DataAlteracao).HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.PerfisPermissos)
                .HasForeignKey(d => d.IdPerfil)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PERFIS_PERMISSOES_PERFIS_FK");

            entity.HasOne(d => d.IdPermissaoNavigation).WithMany(p => p.PerfisPermissos)
                .HasForeignKey(d => d.IdPermissao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PERFIS_PERMISSOES_PERMISSOES_FK");
        });

        modelBuilder.Entity<Permisso>(entity =>
        {
            entity.HasKey(e => e.IdPermissao).HasName("PERMISSOES_PK");

            entity.ToTable("PERMISSOES");

            entity.HasIndex(e => e.Codigo, "PERMISSOES__UN").IsUnique();

            entity.Property(e => e.IdPermissao).HasColumnName("ID_PERMISSAO");
            entity.Property(e => e.Ativo)
                .HasDefaultValue(true)
                .HasColumnName("ATIVO");
            entity.Property(e => e.Codigo)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.DataAlteracao).HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.Descricao)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");
        });

        modelBuilder.Entity<Precovendum>(entity =>
        {
            entity.HasKey(e => e.IdPrecovenda);

            entity.ToTable("PRECOVENDA");

            entity.HasIndex(e => new { e.IdProduto, e.IdCliente }, "UQ_PRECOVENDA_ID_PRODUTO_ID_CLIENTE").IsUnique();

            entity.Property(e => e.IdPrecovenda).HasColumnName("ID_PRECOVENDA");
            entity.Property(e => e.DataAlteracao)
                .HasPrecision(0)
                .HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.Datarevisao)
                .HasPrecision(0)
                .HasColumnName("DATAREVISAO");
            entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");
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
            entity.Property(e => e.Revisao)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("REVISAO");
            entity.Property(e => e.Usurevisao)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("USUREVISAO");
            entity.Property(e => e.Vigencia)
                .HasPrecision(0)
                .HasColumnName("VIGENCIA");

            entity.HasOne(d => d.IdProdutoNavigation).WithMany(p => p.Precovenda)
                .HasForeignKey(d => d.IdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrecoVenda_Produto");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.IdProduto);

            entity.ToTable("PRODUTOS");

            entity.HasIndex(e => e.Codigo, "UX_PRODUTOS_CODIGO").IsUnique();

            entity.Property(e => e.IdProduto).HasColumnName("ID_PRODUTO");
            entity.Property(e => e.Codigo)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.CstIcms)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CST_ICMS");
            entity.Property(e => e.CstIpi)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CST_IPI");
            entity.Property(e => e.DataAlteracao)
                .HasPrecision(0)
                .HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.DataInclusao).HasColumnName("DATA_INCLUSAO");
            entity.Property(e => e.Descricao)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO");
            entity.Property(e => e.FatorEmbalagem).HasColumnName("FATOR_EMBALAGEM");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");
            entity.Property(e => e.Minimo).HasColumnName("MINIMO");
            entity.Property(e => e.Ncm)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("NCM");
            entity.Property(e => e.Obs)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("OBS");
            entity.Property(e => e.ObsEntrada)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("OBS_ENTRADA");
            entity.Property(e => e.ObsNf)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("OBS_NF");
            entity.Property(e => e.Unidade)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("UNIDADE");

            entity.HasOne(d => d.IdUsuarioAlteracaoNavigation).WithMany(p => p.ProdutoIdUsuarioAlteracaoNavigations)
                .HasForeignKey(d => d.IdUsuarioAlteracao)
                .HasConstraintName("PRODUTOS_USUARIOS_FK_ALT");

            entity.HasOne(d => d.IdUsuarioCadastroNavigation).WithMany(p => p.ProdutoIdUsuarioCadastroNavigations)
                .HasForeignKey(d => d.IdUsuarioCadastro)
                .HasConstraintName("PRODUTOS_USUARIOS_FK_CAD");
        });

        modelBuilder.Entity<Tipodocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipodocumento).HasName("TIPODOCUMENTO_PK");

            entity.ToTable("TIPODOCUMENTO");

            entity.HasIndex(e => e.Descricao, "TIPODOCUMENTO__UN").IsUnique();

            entity.Property(e => e.IdTipodocumento).HasColumnName("ID_TIPODOCUMENTO");
            entity.Property(e => e.Descricao)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO");
        });

        modelBuilder.Entity<TokensAtualizacao>(entity =>
        {
            entity.HasKey(e => e.IdTokenAtualizacao).HasName("TOKENS_ATUALIZACAO_PK");

            entity.ToTable("TOKENS_ATUALIZACAO");

            entity.HasIndex(e => new { e.IdUsuario, e.DataExpiracao }, "TOKENS_ATUALIZACAO__IDX_USUARIO");

            entity.Property(e => e.IdTokenAtualizacao).HasColumnName("ID_TOKEN_ATUALIZACAO");
            entity.Property(e => e.DataCadastro)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.DataExpiracao).HasColumnName("DATA_EXPIRACAO");
            entity.Property(e => e.DataRevogacao).HasColumnName("DATA_REVOGACAO");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.IpCriacao)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("IP_CRIACAO");
            entity.Property(e => e.TokenHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TOKEN_HASH");
            entity.Property(e => e.UserAgentCriacao)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("USER_AGENT_CRIACAO");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TokensAtualizacaos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TOKENS_ATUALIZACAO_USUARIOS_FK");
        });

        modelBuilder.Entity<Transportadora>(entity =>
        {
            entity.HasKey(e => e.IdTransportadora).HasName("TRANSPORTADORAS_PK");

            entity.ToTable("TRANSPORTADORAS");

            entity.HasIndex(e => e.Cnpj, "TRANSPORTADORAS__UN").IsUnique();

            entity.Property(e => e.IdTransportadora).HasColumnName("ID_TRANSPORTADORA");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BAIRRO");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("CEP");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("CNPJ");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COMPLEMENTO");
            entity.Property(e => e.Contato)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTATO");
            entity.Property(e => e.DataAlteracao)
                .HasPrecision(0)
                .HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.Endereco)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ENDERECO");
            entity.Property(e => e.Fantasia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FANTASIA");
            entity.Property(e => e.IdMunicipio).HasColumnName("ID_MUNICIPIO");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");
            entity.Property(e => e.InscricaoEstadual)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INSCRICAO_ESTADUAL");
            entity.Property(e => e.Numero).HasColumnName("NUMERO");
            entity.Property(e => e.RazaoSocial)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("RAZAO_SOCIAL");
            entity.Property(e => e.Telefone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TELEFONE");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.Transportadoras)
                .HasForeignKey(d => d.IdMunicipio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TRANSPORTADORAS_MUNICIPIOS_FK");

            entity.HasOne(d => d.IdUsuarioAlteracaoNavigation).WithMany(p => p.TransportadoraIdUsuarioAlteracaoNavigations)
                .HasForeignKey(d => d.IdUsuarioAlteracao)
                .HasConstraintName("TRANSPORTADORAS_USUARIOS_FK_ALT");

            entity.HasOne(d => d.IdUsuarioCadastroNavigation).WithMany(p => p.TransportadoraIdUsuarioCadastroNavigations)
                .HasForeignKey(d => d.IdUsuarioCadastro)
                .HasConstraintName("TRANSPORTADORAS_USUARIOS_FK_CAD");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("USUARIOS_PK");

            entity.ToTable("USUARIOS");

            entity.HasIndex(e => e.Login, "USUARIOS__IDX");

            entity.HasIndex(e => new { e.Email, e.Login }, "USUARIOS__UN").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.Ativo)
                .HasDefaultValue(true)
                .HasColumnName("ATIVO");
            entity.Property(e => e.DataAlteracao)
                .HasPrecision(0)
                .HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOGIN");
            entity.Property(e => e.NomeCompleto)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("NOME_COMPLETO");
            entity.Property(e => e.NomeExibicao)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOME_EXIBICAO");
            entity.Property(e => e.SenhaHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SENHA_HASH");

            entity.HasOne(d => d.IdUsuarioAlteracaoNavigation).WithMany(p => p.InverseIdUsuarioAlteracaoNavigation)
                .HasForeignKey(d => d.IdUsuarioAlteracao)
                .HasConstraintName("USUARIOS_USUARIOS_FK_ALT");

            entity.HasOne(d => d.IdUsuarioCadastroNavigation).WithMany(p => p.InverseIdUsuarioCadastroNavigation)
                .HasForeignKey(d => d.IdUsuarioCadastro)
                .HasConstraintName("USUARIOS_USUARIOS_FK_CAD");
        });

        modelBuilder.Entity<UsuariosPerfi>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdPerfil }).HasName("USUARIOS_PERFIS_PK");

            entity.ToTable("USUARIOS_PERFIS");

            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");
            entity.Property(e => e.DataAlteracao).HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.UsuariosPerfis)
                .HasForeignKey(d => d.IdPerfil)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("USUARIOS_PERFIS_PERFIS_FK");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuariosPerfis)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("USUARIOS_PERFIS_USUARIOS_FK");
        });

        modelBuilder.Entity<Vendedore>(entity =>
        {
            entity.HasKey(e => e.IdVendedor).HasName("VENDEDORES_PK");

            entity.ToTable("VENDEDORES");

            entity.HasIndex(e => e.IdUsuario, "VENDEDORES__UN").IsUnique();

            entity.Property(e => e.IdVendedor).HasColumnName("ID_VENDEDOR");
            entity.Property(e => e.DataAlteracao)
                .HasPrecision(0)
                .HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.DataCadastro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("DATA_CADASTRO");
            entity.Property(e => e.Externo).HasColumnName("EXTERNO");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.IdUsuarioAlteracao).HasColumnName("ID_USUARIO_ALTERACAO");
            entity.Property(e => e.IdUsuarioCadastro).HasColumnName("ID_USUARIO_CADASTRO");
            entity.Property(e => e.Interno).HasColumnName("INTERNO");
            entity.Property(e => e.Observacao)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("OBSERVACAO");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.VendedoreIdUsuarioNavigation)
                .HasForeignKey<Vendedore>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VENDEDORES_USUARIOS_FK");

            entity.HasOne(d => d.IdUsuarioAlteracaoNavigation).WithMany(p => p.VendedoreIdUsuarioAlteracaoNavigations)
                .HasForeignKey(d => d.IdUsuarioAlteracao)
                .HasConstraintName("VENDEDORES_USUARIOS_FK_ALT");

            entity.HasOne(d => d.IdUsuarioCadastroNavigation).WithMany(p => p.VendedoreIdUsuarioCadastroNavigations)
                .HasForeignKey(d => d.IdUsuarioCadastro)
                .HasConstraintName("VENDEDORES_USUARIOS_FK_CAD");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
