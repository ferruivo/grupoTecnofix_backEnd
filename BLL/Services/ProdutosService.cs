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

        #region PrecoVenda
        public async Task<List<PrecoVendaDto>> GetListPrecoVendaAsync(int idCliente, CancellationToken ct)
        {
            return await _repo.GetListPrecoVendaAsync(idCliente, ct);
        }

        public async Task<PrecoVendaDto> GetPrecoVendaByIdAsync(int id, CancellationToken ct)
        {
            var p = await _repo.GetByIdAsync(id, ct);
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
        
        #endregion


    }
}
