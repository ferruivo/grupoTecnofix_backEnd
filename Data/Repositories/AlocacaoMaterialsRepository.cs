using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Estoque;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class AlocacaoMaterialsRepository : IAlocacaoMaterialsRepository
    {
        private readonly AppDbContext _db;

        public AlocacaoMaterialsRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<AlocacaoMaterialListDto>> GetListPagedAsync(int page, int pageSize, int idLote, CancellationToken ct)
        {
            var query =
                from am in _db.AlocacaoMaterials.AsNoTracking()
                join p in _db.Prateleiras.AsNoTracking()
                    on Convert.ToInt32(am.IdPrateleira) equals p.IdPrateleira
                where am.IdLote == idLote
                select new AlocacaoMaterialListDto
                {
                    IdAlocacaoMaterial = am.IdAlocacaoMaterial,
                    IdLote = am.IdLote,
                    Etq = am.Etq,
                    IdPrateleira = Convert.ToInt32(am.IdPrateleira),
                    Prateleira = p.Descricao,
                    Qtd = am.Qtd,
                    Reservado = am.Reservado,
                    DataAlocacao = am.DataAlocacao
                };

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(c => c.Etq)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<AlocacaoMaterialListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }

        public async Task<AlocacaoMaterial?> GetByIdAsync(int id, CancellationToken ct)
        {
            try
            {
                return await _db.AlocacaoMaterials.FirstOrDefaultAsync(x => x.IdAlocacaoMaterial == id, CancellationToken.None);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task AddAsync(AlocacaoMaterial entity, CancellationToken ct)
        {
            return _db.AlocacaoMaterials.AddAsync(entity).AsTask();
        }

        public async Task<List<AlocacaoMaterial>> GetByLoteAsync(int idLote, CancellationToken ct)
        {
            return await _db.AlocacaoMaterials
                .Where(x => x.IdLote == idLote)
                .ToListAsync(ct);
        }

        public void Remove(AlocacaoMaterial entity)
        {
            _db.AlocacaoMaterials.Remove(entity);
        }

        public async Task SaveAsync(CancellationToken ct)
        {
            await _db.SaveChangesAsync(ct);
        }
    }
}
