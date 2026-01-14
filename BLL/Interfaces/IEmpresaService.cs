using GrupoTecnofix_Api.Dtos;
using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.Usuario;
using GrupoTecnofix_Api.Dtos.Vendedor;

namespace GrupoTecnofix_Api.BLL.Interfaces
{
    public interface IEmpresaService
    {
        Task<EmpresaDto> GetAsync(string? search, CancellationToken ct);
        Task<EmpresaDto> GetByIdAsync(int id, CancellationToken ct);
        Task UpdateAsync(int id, EmpresaUpdateDto dto, CancellationToken ct);
    }
}
