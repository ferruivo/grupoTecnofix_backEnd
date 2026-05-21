using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Estoque;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IAlocacaoMaterialsService
    {
        Task<PagedResult<AlocacaoMaterialListDto>> GetPagedAsync(int page, int pageSize, int idLote, CancellationToken ct);
        Task<List<AlocacaoMaterialListDto>> SalvarAlocacoesAsync(AlocacaoMaterialSalvarDto dto, CancellationToken ct);
    }
}
