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

namespace GrupoTecnofix_Api.BLL.Services
{
    public class ClientesService : IClientesService
    {
        private readonly IClientesRepository _repo;
        private readonly IMunicipiosRepository _mun_Repo;
        private readonly IVendedoresRepository _vend_Repo;
        private readonly ITipoDocumentoRepository _td_Repo;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public ClientesService(IClientesRepository repo, IMunicipiosRepository mun_Repo, IVendedoresRepository vend_Repo, ITipoDocumentoRepository td_Repo, ICurrentUserService currentUser, IMapper mapper)
        {
            _repo = repo;
            _mun_Repo = mun_Repo;
            _vend_Repo = vend_Repo;
            _td_Repo = td_Repo;
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
            cli.DataCadastro = DateTime.Now;
            cli.IdUsuarioCadastro = _currentUser.GetUsuarioLogadoId();

            await _repo.AddAsync(cli, ct);
            await _repo.SaveAsync(ct);

            return cli.IdCliente;
        }

        public async Task UpdateAsync(int id, ClienteCreateUpdate dto, CancellationToken ct)
        {
            var c = await _repo.GetByIdAsync(id, ct);
            if (c is null) throw new KeyNotFoundException("Cliente não encontrado.");

            _mapper.Map(dto, c);
            c.DataAlteracao = DateTime.Now;
            c.IdUsuarioAlteracao = _currentUser.GetUsuarioLogadoId();

            await _repo.SaveAsync(ct);
        }
    }
}
