using AutoMapper;
using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Data.Interface;
using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;
using GrupoTecnofix_Api.Models;
using GrupoTecnofix_Api.Utils;

namespace GrupoTecnofix_Api.BLL.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public UsuariosService(IUsuariosRepository repo, IMapper mapper, ICurrentUserService currentUser)
        {
            _repo = repo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<UsuarioListDto>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            return await _repo.GetListPagedAsync(page, pageSize, search, ct);
        }

        public async Task<List<UsuarioListDto>> GetListAsync(string? search, CancellationToken ct)
        {
            return await _repo.GetListAsync(search, ct);
        }

        public async Task<UsuarioDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var u = await _repo.GetByIdAsync(id, ct);
            if (u is null) throw new KeyNotFoundException("Usuário não encontrado.");

            var dto = _mapper.Map<UsuarioDto>(u);

            return dto;
        }

        public async Task<int> CreateAsync(UsuarioCreateDto dto, CancellationToken ct)
        {
            var usuario = _mapper.Map<Usuario>(dto);

            // regra: senha inicial padrão
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456");

            usuario.EnsureCreationAudit(_currentUser);

            await _repo.AddAsync(usuario, ct);
            await _repo.SaveAsync(ct);

            return usuario.IdUsuario;
        }

        public async Task UpdateAsync(int id, UsuarioUpdateDto dto, CancellationToken ct)
        {
            var u = await _repo.GetByIdAsync(id, ct);
            if (u is null) throw new KeyNotFoundException("Usuário não encontrado.");

            _mapper.Map(dto, u);
          
            await _repo.SaveAsync(ct);
        }

        public async Task ResetSenhaAsync(int id, CancellationToken ct)
        {
            var u = await _repo.GetByIdAsync(id, ct);
            if (u is null) throw new KeyNotFoundException("Usuário não encontrado.");

            u.SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456");

            await _repo.SaveAsync(ct);
        }
    }
}
