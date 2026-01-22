using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IClientesRepository
    {
        Task<PagedResult<ClienteListDto>> GetListPagedAsync(int page, int pageSize, string? search, CancellationToken ct);
        Task<List<ClienteExcelDto>> GetListExcelAsync(string? search, CancellationToken ct);
        Task<ClienteDto?> GetClienteDtoByIdAsync(int id, CancellationToken ct);
        Task<Cliente?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(Cliente entity, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);

        Task<List<OrigemCadastroDto>> GetListOrigemAsync(string? search, CancellationToken ct);
        Task<List<TipoDocumentoDto>> GetListTipoDocumentoAsync(string? search, CancellationToken ct);

        Task<List<ClienteFornecedor>> GetListRestricaoFornecedorAsync(int idCliente, CancellationToken ct);
        Task AddAsync(ClienteFornecedor entity, CancellationToken ct);
        Task DeleteRestricaoFornecedorAsync(ClienteFornecedor cf, CancellationToken ct);

    }
}
