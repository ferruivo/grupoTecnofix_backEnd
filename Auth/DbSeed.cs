using GrupoTecnofix_Api.Data;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Auth
{
    public static class DbSeed
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            // PERFIL ADMIN_SISTEMA
            var perfilAdmin = await db.Perfis.FirstOrDefaultAsync(p => p.Nome == "ADMIN_SISTEMA");
            if (perfilAdmin == null)
            {
                perfilAdmin = new Perfi { Nome = "ADMIN_SISTEMA" };
                db.Perfis.Add(perfilAdmin);
                await db.SaveChangesAsync();
            }

            // USUÁRIO ADMIN
            var admin = await db.Usuarios.FirstOrDefaultAsync(u => u.Login == "admin");
            if (admin == null)
            {
                admin = new Usuario
                {
                    NomeCompleto = "Administrador do Sistema",
                    NomeExibicao = "Admin",
                    Login = "admin",
                    Email = "admin@empresa.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Ativo = true
                };

                db.Usuarios.Add(admin);
                await db.SaveChangesAsync();
            }

            // VÍNCULO USUÁRIO x PERFIL
            var vinculo = await db.UsuariosPerfis
                .FirstOrDefaultAsync(x => x.IdUsuario == admin.IdUsuario && x.IdPerfil == perfilAdmin.IdPerfil);

            if (vinculo == null)
            {
                db.UsuariosPerfis.Add(new UsuariosPerfi
                {
                    IdUsuario = admin.IdUsuario,
                    IdPerfil = perfilAdmin.IdPerfil
                });

                await db.SaveChangesAsync();
            }
        }
    }
}
