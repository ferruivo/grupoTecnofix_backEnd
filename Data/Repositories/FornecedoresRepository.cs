using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Condições_Pagamento;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class FornecedoresRepository : IFornecedoresRepository
    {
        private readonly AppDbContext _db;

        public FornecedoresRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<FornecedorListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            var query = _db.Fornecedores.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(f =>
                    (f.Razaosocial ?? "").Contains(s) ||
                    (f.Fantasia ?? "").Contains(s) ||
                    f.Cpfcnpj.Contains(s) ||
                    (f.Contato ?? "").Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(f => f.Razaosocial)
                .ThenBy(f => f.Fantasia)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new FornecedorListDto
                {
                    IdFornecedor = f.IdFornecedor,
                    RazaoSocial = f.Razaosocial,
                    Fantasia = f.Fantasia,
                    CpfCnpj = f.Cpfcnpj,
                    Contato = f.Contato,
                })
                .ToListAsync(ct);

            return new PagedResult<FornecedorListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<List<FornecedorListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            var query = _db.Fornecedores.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(f =>
                    (f.Razaosocial ?? "").Contains(s) ||
                    (f.Fantasia ?? "").Contains(s) ||
                    f.Cpfcnpj.Contains(s) ||
                    (f.Ie ?? "").Contains(s));
            }

            var items = await query
                .OrderBy(f => f.Razaosocial)
                .ThenBy(f => f.Fantasia)
                .Select(f => new FornecedorListDto
                {
                    IdFornecedor = f.IdFornecedor,
                    RazaoSocial = f.Razaosocial,
                    Fantasia = f.Fantasia,
                    CpfCnpj = f.Cpfcnpj,
                    Contato = f.Contato,
                })
                .ToListAsync(ct);

            return items;
        }

        public async Task<List<FornecedorExcelDto>> GetListExcelAsync(string? search, CancellationToken ct)
        {
            var query = _db.Fornecedores.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(f =>
                    f.Fantasia.Contains(s) ||
                    f.Contato.Contains(s) ||
                    f.Razaosocial.Contains(s) ||
                    f.Cpfcnpj.Contains(s));
            }

            var total = await query.CountAsync(ct);

            var items = await (from f in query
                               join m in _db.Municipios on f.IdMunicipio equals m.IdMunicipio
                               orderby f.Fantasia
                               select new FornecedorExcelDto
                               {
                                   RazaoSocial = f.Razaosocial,
                                   Fantasia = f.Fantasia,
                                   Contato = f.Contato,
                                   CpfCnpj = f.Cpfcnpj,
                                   Municipio = m.Nome,
                                   UF = m.Uf
                               }).ToListAsync(ct);
            return items;
        }
        public Task<Fornecedore?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Fornecedores.FirstOrDefaultAsync(x => x.IdFornecedor == id, ct);

        public async Task AddAsync(Fornecedore fornecedor, CancellationToken ct)
            => await _db.Fornecedores.AddAsync(fornecedor, ct);

        public Task SaveAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
