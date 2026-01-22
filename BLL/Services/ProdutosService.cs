using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Fornecedor;
using GrupoTecnofix_Api.Dtos.Produto;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class ProdutosService : IProdutosService
    {
        private readonly IProdutosRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public ProdutosService(IProdutosRepository repo, IMapper mapper, ICurrentUserService currentUser)
        {
            _repo = repo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<ProdutoListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListPagedAsync(page, pageSize, search, ct);
        }

        public async Task<List<ProdutoListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListAsync(search, ct);
        }

        public async Task<ProdutoDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var p = await _repo.GetByIdAsync(id, ct);
            if (p is null) throw new KeyNotFoundException("Produto não encontrado.");

            var dto = _mapper.Map<ProdutoDto>(p);

            return dto;
        }

        public async Task<int> CreateAsync(ProdutoCreateUpdate dto, CancellationToken ct)
        {
            var p = _mapper.Map<Produto>(dto);

            p.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(p, ct);
            await _repo.SaveAsync(ct);

            return p.IdProduto;
        }

        public async Task UpdateAsync(int id, ProdutoCreateUpdate dto, CancellationToken ct)
        {
            var p = await _repo.GetByIdAsync(id, ct);
            if (p is null) throw new KeyNotFoundException("Produto não encontrado.");

            _mapper.Map(dto, p);
            p.EnsureUpdateAudit(_currentUser);

            await _repo.SaveAsync(ct);
        }

        public async Task<byte[]> ExportListToExcelAsync(IEnumerable<ProdutoListDto> list, CancellationToken ct)
        {
            return await Task.Run(() => Helpers.ExcelExporter.ExportToExcel(list, "Produtos"), ct);
        }

        public async Task<byte[]> ExportListToExcelAsync(string? search, CancellationToken ct)
        {
            var list = await _repo.GetListPagedAsync(1, 1000, search, ct);
            // use helper to export
            return await Task.Run(() => Helpers.ExcelExporter.ExportToExcel(list.Items, "produtos"), ct);
        }

        #region PrecoVenda
        public async Task<List<PrecoVendaDto>> GetListPrecoVendaAsync(int idCliente, CancellationToken ct)
        {
            return await _repo.GetListPrecoVendaAsync(idCliente, ct);
        }

        public async Task<PrecoVendaDto> GetPrecoVendaByIdAsync(int id, CancellationToken ct)
        {
            var p = await _repo.GetPrecoVendaByIdAsync(id, ct);
            if (p is null) throw new KeyNotFoundException("Preço venda não encontrado.");

            var dto = _mapper.Map<PrecoVendaDto>(p);

            return dto;
        }

        public async Task<int> CreateAsync(PrecoVendaCreateUpdateDto dto, CancellationToken ct)
        {
            var p = _mapper.Map<Precovendum>(dto);

            p.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(p, ct);
            await _repo.SaveAsync(ct);

            return p.IdProduto;
        }

        public async Task UpdateAsync(int id, PrecoVendaCreateUpdateDto dto, CancellationToken ct)
        {
            var p = await _repo.GetPrecoVendaByIdAsync(id, ct);
            if (p is null) throw new KeyNotFoundException("Preço venda não encontrado.");

            _mapper.Map(dto, p);
            p.EnsureUpdateAudit(_currentUser);

            await _repo.SaveAsync(ct);
        }

        public async Task UpdatePrecoVendaGeral(PrecoVendaReajusteGeralDto dto, CancellationToken ct)
        {
            var p = await _repo.GetPrecoVendaByIdAsync(dto.Id_Cliente, ct);
            if (p is null) throw new KeyNotFoundException("Preço venda não encontrado.");

            var listPv = await _repo.GetListPrecoVendaAsync(p.IdCliente, ct);

            foreach (var item in listPv) 
            {
                _mapper.Map(item, p);

                p.Precoantigo = item.Precoantigo;
                p.Preco = item.Preco * dto.Porcentagem;
                p.DataAlteracao = DateTime.Now;
                p.EnsureUpdateAudit(_currentUser);

                await _repo.SaveAsync(ct);
            }

        }

        public async Task<byte[]> ExportPrecoVendaToExcelAsync(int idCliente, CancellationToken ct)
        {
            var items = await _repo.GetListPrecoVendaAsync(idCliente, ct);
            return await Task.Run(() => Helpers.ExcelExporter.ExportToExcel(items, "PrecoVenda"), ct);
        }

        #endregion

        #region PrecoCompra
        public async Task<List<PrecoCompraDto>> GetListPrecoCompraAsync(int idFornecedor, CancellationToken ct)
        {
            return await _repo.GetListPrecoCompraAsync(idFornecedor, ct);
        }

        public async Task<PrecoCompraDto> GetPrecoCompraByIdAsync(int id, CancellationToken ct)
        {
            var p = await _repo.GetPrecoCompraByIdAsync(id, ct);
            if (p is null) throw new KeyNotFoundException("Preço compra não encontrado.");

            var dto = _mapper.Map<PrecoCompraDto>(p);

            return dto;
        }

        public async Task<int> CreateAsync(PrecoCompraCreateUpdateDto dto, CancellationToken ct)
        {
            var p = _mapper.Map<Precocompra>(dto);

            p.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(p, ct);
            await _repo.SaveAsync(ct);

            return p.IdProduto;
        }

        public async Task UpdateAsync(int id, PrecoCompraCreateUpdateDto dto, CancellationToken ct)
        {
            var p = await _repo.GetPrecoCompraByIdAsync(id, ct);
            if (p is null) throw new KeyNotFoundException("Preço compra não encontrado.");

            _mapper.Map(dto, p);
            p.EnsureUpdateAudit(_currentUser);

            await _repo.SaveAsync(ct);
        }

        public async Task UpdatePrecoCompraGeral(PrecoCompraReajusteGeralDto dto, CancellationToken ct)
        {
            var p = await _repo.GetPrecoCompraByIdAsync(dto.Id_Fornecedor, ct);
            if (p is null) throw new KeyNotFoundException("Preço compra não encontrado.");

            var listPv = await _repo.GetListPrecoCompraAsync(p.IdFornecedor, ct);

            foreach (var item in listPv)
            {
                _mapper.Map(item, p);

                p.Precoantigo = item.Precoantigo;
                p.Preco = item.Preco * dto.Porcentagem;
                p.DataAlteracao = DateTime.Now;
                p.EnsureUpdateAudit(_currentUser);

                await _repo.SaveAsync(ct);
            }

        }

        public async Task<byte[]> ExportPrecoCompraToExcelAsync(int idFornecedor, CancellationToken ct)
        {
            var items = await _repo.GetListPrecoCompraAsync(idFornecedor, ct);
            return await Task.Run(() => Helpers.ExcelExporter.ExportToExcel(items, "PrecoCompra"), ct);
        }

        #endregion

        #region KitProduto
        public async Task<List<ProdutoKitDto>> GetListProdutoKitAsync(int idProduto, CancellationToken ct)
        {
            return await _repo.GetListProdutoKitAsync(idProduto, ct);
        }

        public async Task<ProdutoKitDto> GetProdutoKitByIdAsync(int idProdutoKit, int idProduto, CancellationToken ct)
        {
            var p = await _repo.GetProdutoKitByIdAsync(idProdutoKit, idProduto, ct);
            if (p is null) throw new KeyNotFoundException("Produto kit não encontrado.");

            var dto = _mapper.Map<ProdutoKitDto>(p);

            return dto;
        }

        public async Task<int> CreateAsync(ProdutoKitCreateUpdateDto dto, CancellationToken ct)
        {
            var p = _mapper.Map<ProdutoKitIten>(dto);

            p.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(p, ct);
            await _repo.SaveAsync(ct);

            return p.IdProduto;
        }

        public async Task UpdateAsync(ProdutoKitCreateUpdateDto dto, CancellationToken ct)
        {
            var p = await _repo.GetProdutoKitByIdAsync(dto.IdProdutoKit, dto.IdProduto, ct);
            if (p is null) throw new KeyNotFoundException("Produto kit não encontrado.");

            _mapper.Map(dto, p);
            p.EnsureUpdateAudit(_currentUser);

            await _repo.SaveAsync(ct);
        }

        public async Task DeleteProdutoKitAsync(int IdProdutoKit, int idProduto, CancellationToken ct)
        {
            var k = await _repo.GetProdutoKitByIdAsync(IdProdutoKit, idProduto, ct);

            if (k == null)
                throw new KeyNotFoundException("Item não encontrado.");

            await _repo.DeleteProdutoKitAsync(k, ct);

        }
        #endregion
    }
}
