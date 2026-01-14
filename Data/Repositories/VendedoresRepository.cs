using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class VendedoresRepository : IVendedoresRepository
    {
        private readonly AppDbContext _db;

        public VendedoresRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<VendedorListDto>> GetListAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query =
                from v in _db.Vendedores.AsNoTracking()
                join u in _db.Usuarios.AsNoTracking()
                    on v.IdUsuario equals u.IdUsuario
                select new { v, u };

            // filtro (inclui nome do usuário)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();

                query = query.Where(x =>
                    x.u.NomeCompleto.Contains(s) ||
                    (x.v.Observacao != null && x.v.Observacao.Contains(s))
                );
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(x => x.v.IdVendedor)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new VendedorListDto
                {
                    IdVendedor = x.v.IdVendedor,
                    Interno = x.v.Interno,
                    Externo = x.v.Externo,
                    Observacao = x.v.Observacao,

                    Usuario = new UsuarioDto
                    {
                        NomeCompleto = x.u.NomeCompleto
                    }
                })
                .ToListAsync(ct);

            return new PagedResult<VendedorListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public Task<VendedorDto?> GetByIdAsync(int id, CancellationToken ct)
     => (
         from v in _db.Vendedores.AsNoTracking()
         join u in _db.Usuarios.AsNoTracking()
             on v.IdUsuario equals u.IdUsuario
         where v.IdVendedor == id
         select new VendedorDto
         {
             IdVendedor = v.IdVendedor,
             IdUsuario = v.IdUsuario,
             Interno = v.Interno,
             Externo = v.Externo,
             Observacao = v.Observacao ?? "",

             Usuario = new UsuarioDto
             {
                 IdUsuario = u.IdUsuario,
                 NomeCompleto = u.NomeCompleto
             }
         }
     ).FirstOrDefaultAsync(ct);

        public async Task<Vendedore?> GetAsync(int id, CancellationToken ct)
        {
            return await _db.Vendedores.FirstOrDefaultAsync(x => x.IdVendedor == id, ct);
        }


        public async Task AddAsync(Vendedore vendedore, CancellationToken ct)
            => await _db.Vendedores.AddAsync(vendedore, ct);

        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
