using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;


namespace GrupoTecnofix_Api.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Perfil> Perfis => Set<Perfil>();
        public DbSet<Permissao> Permissoes => Set<Permissao>();
        public DbSet<UsuarioPerfil> UsuariosPerfis => Set<UsuarioPerfil>();
        public DbSet<PerfilPermissao> PerfisPermissoes => Set<PerfilPermissao>();
        public DbSet<TokenAtualizacao> TokensAtualizacao => Set<TokenAtualizacao>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Usuario>().ToTable("USUARIOS").HasKey(x => x.IdUsuario);
            mb.Entity<Usuario>().Property(x => x.IdUsuario).HasColumnName("ID_USUARIO");
            mb.Entity<Usuario>().Property(x => x.NomeCompleto).HasColumnName("NOME_COMPLETO");
            mb.Entity<Usuario>().Property(x => x.NomeExibicao).HasColumnName("NOME_EXIBICAO");
            mb.Entity<Usuario>().Property(x => x.Email).HasColumnName("EMAIL");
            mb.Entity<Usuario>().Property(x => x.Login).HasColumnName("LOGIN");
            mb.Entity<Usuario>().Property(x => x.SenhaHash).HasColumnName("SENHA_HASH");
            mb.Entity<Usuario>().Property(x => x.Ativo).HasColumnName("ATIVO");

            mb.Entity<Perfil>().ToTable("PERFIS").HasKey(x => x.IdPerfil);
            mb.Entity<Perfil>().Property(x => x.IdPerfil).HasColumnName("ID_PERFIL");
            mb.Entity<Perfil>().Property(x => x.Nome).HasColumnName("NOME");

            mb.Entity<Permissao>().ToTable("PERMISSOES").HasKey(x => x.IdPermissao);
            mb.Entity<Permissao>().Property(x => x.IdPermissao).HasColumnName("ID_PERMISSAO");
            mb.Entity<Permissao>().Property(x => x.Codigo).HasColumnName("CODIGO");

            mb.Entity<UsuarioPerfil>().ToTable("USUARIOS_PERFIS").HasKey(x => new { x.IdUsuario, x.IdPerfil });
            mb.Entity<UsuarioPerfil>().Property(x => x.IdUsuario).HasColumnName("ID_USUARIO");
            mb.Entity<UsuarioPerfil>().Property(x => x.IdPerfil).HasColumnName("ID_PERFIL");

            mb.Entity<PerfilPermissao>().ToTable("PERFIS_PERMISSOES").HasKey(x => new { x.IdPerfil, x.IdPermissao });
            mb.Entity<PerfilPermissao>().Property(x => x.IdPerfil).HasColumnName("ID_PERFIL");
            mb.Entity<PerfilPermissao>().Property(x => x.IdPermissao).HasColumnName("ID_PERMISSAO");

            mb.Entity<TokenAtualizacao>().ToTable("TOKENS_ATUALIZACAO").HasKey(x => x.IdTokenAtualizacao);
            mb.Entity<TokenAtualizacao>().Property(x => x.IdTokenAtualizacao).HasColumnName("ID_TOKEN_ATUALIZACAO");
            mb.Entity<TokenAtualizacao>().Property(x => x.IdUsuario).HasColumnName("ID_USUARIO");
            mb.Entity<TokenAtualizacao>().Property(x => x.TokenHash).HasColumnName("TOKEN_HASH");
            mb.Entity<TokenAtualizacao>().Property(x => x.DataExpiracao).HasColumnName("DATA_EXPIRACAO");
            mb.Entity<TokenAtualizacao>().Property(x => x.DataRevogacao).HasColumnName("DATA_REVOGACAO");
            mb.Entity<TokenAtualizacao>().Property(x => x.DataCadastro).HasColumnName("DATA_CADASTRO");
            mb.Entity<TokenAtualizacao>().Property(x => x.IpCriacao).HasColumnName("IP_CRIACAO");
            mb.Entity<TokenAtualizacao>().Property(x => x.UserAgentCriacao).HasColumnName("USER_AGENT_CRIACAO");
        }
    }
}
