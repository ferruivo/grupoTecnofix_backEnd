using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Dtos.Produto;
using GrupoTecnofix_Api.Models;
using System.Threading.Tasks;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IProdutosService
    {
        Task<PagedResult<ProdutoListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<ProdutoListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<ProdutoDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(ProdutoCreateUpdate dto, CancellationToken ct);
        Task UpdateAsync(int id, ProdutoCreateUpdate dto, CancellationToken ct);

        Task<List<PrecoVendaDto>> GetListPrecoVendaAsync(int idCliente, CancellationToken ct);
        Task<PrecoVendaDto> GetPrecoVendaByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(PrecoVendaCreateUpdateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, PrecoVendaCreateUpdateDto dto, CancellationToken ct);
        Task UpdatePrecoVendaGeral(PrecoVendaReajusteGeralDto dto, CancellationToken ct);
        
    }
}
