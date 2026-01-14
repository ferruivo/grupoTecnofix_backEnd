using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Transportadoras;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class VendedoresService : IVendedoresService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IVendedoresRepository _repo;
        private readonly IMapper _mapper;

        public VendedoresService(ICurrentUserService currentUser,IVendedoresRepository repo, IMapper mapper)
        {
            _currentUser = currentUser;
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<VendedorListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListAsync(page, pageSize, search, ct);
        }

        public async Task<VendedorDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var v = await _repo.GetByIdAsync(id, ct);
            if (v is null) throw new KeyNotFoundException("Vendedor não encontrado.");

            return new VendedorDto
            {
                IdVendedor = v.IdVendedor,
                IdUsuario = v.IdUsuario,
                Usuario = v.Usuario,
                Interno = v.Interno,
                Externo = v.Externo,
                Observacao = v.Observacao
            };
        }

        public async Task<int> CreateAsync(VendedorCreateDto dto, CancellationToken ct)
        {
            var existente = await _repo.GetByIdAsync(dto.IdUsuario, ct);
            if (existente is not null)
                throw new ConflictException("Este usuário já esta cadastrado como vendedor.");

            var vend = _mapper.Map<Vendedore>(dto);

            vend.DataCadastro = DateTime.Now;
            vend.IdUsuarioCadastro = _currentUser.GetUsuarioLogadoId();

            await _repo.AddAsync(vend, ct);
            await _repo.SaveAsync(ct);

            return vend.IdVendedor;
        }

        public async Task UpdateAsync(int id, VendedorUpdateDto dto, CancellationToken ct)
        {
            var vend = await _repo.GetAsync(id, ct);
            if (vend is null) throw new KeyNotFoundException("Vendedor não encontrado.");

            _mapper.Map(dto, vend);
            
            vend.DataAlteracao = DateTime.Now;
            vend.IdUsuarioAlteracao = _currentUser.GetUsuarioLogadoId();

            await _repo.SaveAsync(ct);
        }
    }
}
