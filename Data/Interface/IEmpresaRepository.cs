using GrupoTecnofix_Api.Dtos.Empresa;
using GrupoTecnofix_Api.Dtos.Municipios;
using GrupoTecnofix_Api.Models;

namespace GrupoTecnofix_Api.Data.Interface
{
    public interface IEmpresaRepository
    {
        Task<EmpresaDto> GetAsync(string? search, CancellationToken ct);
        Task<Empresa?> GetByIdAsync(int id, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
