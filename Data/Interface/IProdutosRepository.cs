using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Produto;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IProdutosRepository
    {
        Task<PagedResult<ProdutoListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<ProdutoListDto>> GetListAsync(string? search, CancellationToken ct);
        Task<Produto?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Produto entity, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);

        Task<List<PrecoVendaDto>> GetListPrecoVendaAsync(int idCliente, CancellationToken ct);
        Task<Precovendum?> GetPrecoVendaByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Precovendum entity, CancellationToken ct);

        Task<List<PrecoCompraDto>> GetListPrecoCompraAsync(int idFornecedor, CancellationToken ct);
        Task<Precocompra?> GetPrecoCompraByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Precocompra entity, CancellationToken ct);

        Task<List<ProdutoKitDto>> GetListProdutoKitAsync(int idProduto, CancellationToken ct);
        Task<ProdutoKitIten?> GetProdutoKitByIdAsync(int id, int idProduto, CancellationToken ct);
        Task AddAsync(ProdutoKitIten entity, CancellationToken ct);
        Task DeleteProdutoKitAsync(ProdutoKitIten p, CancellationToken ct);
    }
}
