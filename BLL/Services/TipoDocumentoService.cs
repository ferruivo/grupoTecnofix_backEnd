using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class TipoDocumentoService : ITipoDocumentoService
    {
        private readonly ITipoDocumentoRepository _repo;
        public TipoDocumentoService(ITipoDocumentoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<TipoDocumentoListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListAsync(search, ct);
        }
    }
}
