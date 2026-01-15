using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using Microsoft.EntityFrameworkCore;

namespace GrupoTecnofix_Api.Data.Repositories
{
    public class TipoDocumentoRepository : ITipoDocumentoRepository
    {
        private readonly AppDbContext _db;

        public TipoDocumentoRepository(AppDbContext db) => _db = db;
        public async Task<List<TipoDocumentoListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            var query = from t in _db.Tipodocumentos.AsNoTracking()
                        select new TipoDocumentoListDto
                        {
                            IdTipoDocumento = t.IdTipodocumento,
                            Descricao = t.Descricao,
                        };

            return await query.ToListAsync(ct);
                
        }
    }
}
