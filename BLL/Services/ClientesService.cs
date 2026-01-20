using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Cliente;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Dtos.TipoDocumento;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class ClientesService : IClientesService
    {
        private readonly IClientesRepository _repo;
        
       
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public ClientesService(IClientesRepository repo, ICurrentUserService currentUser, IMapper mapper)
        {
            _repo = repo;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<PagedResult<ClienteListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListPagedAsync(page, pageSize, search, ct);
        }

        public async Task<ClienteDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var c = await _repo.GetClienteDtoByIdAsync(id, ct);
            if (c is null) throw new KeyNotFoundException("Cliente não encontrado.");

            return c;
        }

        public async Task<int> CreateAsync(ClienteCreateUpdate dto, CancellationToken ct)
        {
            var cli = _mapper.Map<Cliente>(dto);
            cli.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(cli, ct);
            await _repo.SaveAsync(ct);

            return cli.IdCliente;
        }

        public async Task UpdateAsync(int id, ClienteCreateUpdate dto, CancellationToken ct)
        {
            var c = await _repo.GetByIdAsync(id, ct);
            if (c is null) throw new KeyNotFoundException("Cliente não encontrado.");

            _mapper.Map(dto, c);
            c.EnsureUpdateAudit(_currentUser);

            await _repo.SaveAsync(ct);
        }

        public async Task<List<OrigemCadastroDto>> GetListOrigemAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListOrigemAsync(search, ct);
        }

        public async Task<List<TipoDocumentoDto>> GetListTipoDocumentoAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListTipoDocumentoAsync(search, ct);
        }

        public async Task<byte[]> ExportListToExcelAsync(string? search, CancellationToken ct)
        {
            var list = await _repo.GetListPagedAsync(1,1000,search, ct);
            // use helper to export
            return await Task.Run(() => Helpers.ExcelExporter.ExportToExcel(list.Items, "Clientes"), ct);
        }

        public async Task<List<ClienteFornecedor>> GetListRestricaoFornecedorAsync(int idCliente, CancellationToken ct)
        {
            return await _repo.GetListRestricaoFornecedorAsync(idCliente, ct);
        }

        public async Task<int> AddAsync(ClienteFornecedor cf, CancellationToken ct)
        {

            cf.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(cf, ct);
            await _repo.SaveAsync(ct);

            return cf.IdCliente;
        }

       
        public async Task DeleteRestricaoFornecedorAsync(int idCliente, int idFornecedor, CancellationToken ct)
        {
            var list = await _repo.GetListRestricaoFornecedorAsync(idCliente, ct);

            ClienteFornecedor cf = list.Where(x => x.IdFornecedor == idFornecedor).FirstOrDefault();
            if (cf == null)
                throw new KeyNotFoundException("Restrição não encontrada.");

            await _repo.DeleteRestricaoFornecedorAsync(cf, ct);

        }
    }
}
